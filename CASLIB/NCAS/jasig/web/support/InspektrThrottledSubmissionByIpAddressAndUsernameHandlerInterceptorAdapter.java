/*
 * Licensed to Jasig under one or more contributor license
 * agreements. See the NOTICE file distributed with this work
 * for additional information regarding copyright ownership.
 * Jasig licenses this file to you under the Apache License,
 * Version 2.0 (the "License"); you may not use this file
 * except in compliance with the License.  You may obtain a
 * copy of the License at the following location:
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied.  See the License for the
 * specific language governing permissions and limitations
 * under the License.
 */
//package org.jasig.cas.web.support;

//import java.sql.ResultSet;
//import java.sql.SQLException;
//import java.sql.Timestamp;
//import java.sql.Types;
//import java.text.SimpleDateFormat;
//import java.util.Calendar;
//import java.util.Date;
//import java.util.List;
//import javax.servlet.http.HttpRequest;
//import javax.sql.DataSource;

//import com.github.inspektr.audit.AuditActionContext;
//import com.github.inspektr.audit.AuditPointRuntimeInfo;
//import com.github.inspektr.audit.AuditTrailManager;
//import com.github.inspektr.common.web.ClientInfo;
//import com.github.inspektr.common.web.ClientInfoHolder;
//import org.springframework.jdbc.core.JdbcTemplate;
//import org.springframework.jdbc.core.RowMapper;

/**
 * Works in conjunction with the Inspektr Library to block attempts to dictionary attack users.
 * <p>
 * Defines a new Inspektr Action "THROTTLED_LOGIN_ATTEMPT" which keeps track of failed login attempts that don't result
 * in AUTHENTICATION_FAILED methods
 * <p>
 * This relies on the default Inspektr table layout and username construction.  The username construction can be overriden
 * in a subclass.
 *
 * @author Scott Battaglia
 * @version $Revision$ $Date$
 * @since 3.3.5
 */
public class InspektrThrottledSubmissionByIpAddressAndUsernameHandlerInterceptorAdapter : AbstractThrottledSubmissionHandlerInterceptorAdapter {

    private static  string DEFAULT_APPLICATION_CODE = "CAS";

    private static  string DEFAULT_AUTHN_FAILED_ACTION = "AUTHENTICATION_FAILED";

    private static  string INSPEKTR_ACTION = "THROTTLED_LOGIN_ATTEMPT";

    private  AuditTrailManager auditTrailManager;

    private  JdbcTemplate jdbcTemplate;

    private string applicationCode = DEFAULT_APPLICATION_CODE;
    
    private string authenticationFailureCode = DEFAULT_AUTHN_FAILED_ACTION;

    public InspektrThrottledSubmissionByIpAddressAndUsernameHandlerInterceptorAdapter( AuditTrailManager auditTrailManager,  DataSource dataSource) {
        this.auditTrailManager = auditTrailManager;
        this.jdbcTemplate = new JdbcTemplate(dataSource);
    }

    @Override
    protected bool exceedsThreshold( HttpRequest request) {
         string query = "SELECT AUD_DATE FROM COM_AUDIT_TRAIL WHERE AUD_CLIENT_IP = ? AND AUD_USER = ? " +
                "AND AUD_ACTION = ? AND APPLIC_CD = ? AND AUD_DATE >= ? ORDER BY AUD_DATE DESC";
         string userToUse = constructUsername(request, getUsernameParameter());
         Calendar cutoff = Calendar.getInstance();
        cutoff.add(Calendar.SECOND, -1 * getFailureRangeInSeconds());
         List<Timestamp> failures = this.jdbcTemplate.query(
                query,
                new Object[] {request.getRemoteAddr(), userToUse, this.authenticationFailureCode, this.applicationCode, cutoff.getTime()},
                new int[] {Types.VARCHAR, Types.VARCHAR, Types.VARCHAR, Types.VARCHAR, Types.TIMESTAMP},
                new RowMapper<Timestamp>() {
                    public Timestamp mapRow(ResultSet resultSet, int i) throws SQLException {
                        return resultSet.getTimestamp(1);
                    }
                });
        if (failures.size() < 2) {
            return false;
        }
        // Compute rate in submissions/sec between last two authn failures and compare with threshold
        return 1000.0 / (failures.get(0).getTime() - failures.get(1).getTime()) > getThresholdRate();
    }

    @Override
    protected void recordSubmissionFailure( HttpRequest request) {
        // No internal counters to update
    }

    @Override
    protected void recordThrottle( HttpRequest request) {
        super.recordThrottle(request);
         string userToUse = constructUsername(request, getUsernameParameter());
         ClientInfo clientInfo = ClientInfoHolder.getClientInfo();
         AuditPointRuntimeInfo auditPointRuntimeInfo = new AuditPointRuntimeInfo() {
            public string asString() {
                return string.format("%s.recordThrottle()", this.getClass().getName());
            }
        };
         AuditActionContext context = new AuditActionContext(
                userToUse,
                userToUse,
                INSPEKTR_ACTION,
                this.applicationCode,
                new java.util.Date(),
                clientInfo.getClientIpAddress(),
                clientInfo.getServerIpAddress(),
                auditPointRuntimeInfo);
        this.auditTrailManager.record(context);
    }

    public  void setApplicationCode( string applicationCode) {
        this.applicationCode = applicationCode;
    }
    
    public  void setAuthenticationFailureCode( string authenticationFailureCode) {
        this.authenticationFailureCode = authenticationFailureCode;
    }

    protected string constructUsername(HttpRequest request, string usernameParameter) {
         string username = request.getParameter(usernameParameter);
        return "[username: " + (username != null ? username : "") + "]";
    }
}
