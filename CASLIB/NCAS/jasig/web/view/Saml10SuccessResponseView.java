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
//package org.jasig.cas.web.view;

//import java.util.Collection;
//import java.util.Map;
//import java.util.Map.Entry;
//import javax.validation.constraints.Min;
//import javax.validation.constraints.NotNull;

//import org.jasig.cas.authentication.Authentication;
//import org.jasig.cas.authentication.SamlAuthenticationMetaDataPopulator;
//import org.jasig.cas.authentication.principal.RememberMeCredentials;
//import org.jasig.cas.authentication.principal.Service;
//import org.joda.time.DateTime;
//import org.opensaml.saml1.core.Assertion;
//import org.opensaml.saml1.core.Attribute;
//import org.opensaml.saml1.core.AttributeStatement;
//import org.opensaml.saml1.core.AttributeValue;
//import org.opensaml.saml1.core.Audience;
//import org.opensaml.saml1.core.AudienceRestrictionCondition;
//import org.opensaml.saml1.core.AuthenticationStatement;
//import org.opensaml.saml1.core.Conditions;
//import org.opensaml.saml1.core.ConfirmationMethod;
//import org.opensaml.saml1.core.NameIdentifier;
//import org.opensaml.saml1.core.Response;
//import org.opensaml.saml1.core.StatusCode;
//import org.opensaml.saml1.core.Subject;
//import org.opensaml.saml1.core.SubjectConfirmation;
//import org.opensaml.xml.schema.XSString;
//import org.opensaml.xml.schema.impl.XSStringBuilder;

/**
 * Implementation of a view to return a SAML SOAP response and assertion, based on
 * the SAML 1.1 specification.
 * <p>
 * If an AttributePrincipal is supplied, then the assertion will include the
 * attributes from it (assuming a string key/Object value pair). The only
 * Authentication attribute it will look at is the authMethod (if supplied).
 * <p>
 * Note that this class will currently not handle proxy authentication.
 * <p>
 * Note: This class currently expects a bean called "ServiceRegistry" to exist.
 * 
 * @author Scott Battaglia
 * @author Marvin S. Addison
 * @since 3.1
 */
public  class Saml10SuccessResponseView : AbstractSaml10ResponseView {

    /** Namespace for custom attributes. */
    private static  string NAMESPACE = "http://www.ja-sig.org/products/cas/";

    private static  string REMEMBER_ME_ATTRIBUTE_NAME = "longTermAuthenticationRequestTokenUsed";

    private static  string REMEMBER_ME_ATTRIBUTE_VALUE = "true";

    private static  string CONFIRMATION_METHOD = "urn:oasis:names:tc:SAML:1.0:cm:artifact";

    private  XSStringBuilder attrValueBuilder = new XSStringBuilder();

    /** The issuer, generally the hostname. */
    //@NotNull
    private string issuer;

    /** The amount of time in milliseconds this is valid for. */
    @Min(1000)
    private long issueLength = 30000;

    //@NotNull
    private string rememberMeAttributeName = REMEMBER_ME_ATTRIBUTE_NAME;

    @Override
    protected void prepareResponse( Response response,  Map<string, Object> model) {
         Authentication authentication = getAssertionFrom(model).getChainedAuthentications().get(0);
         DateTime issuedAt = response.getIssueInstant();
         Service service = getAssertionFrom(model).getService();
         bool isRemembered = (
                authentication.getAttributes().get(RememberMeCredentials.AUTHENTICATION_ATTRIBUTE_REMEMBER_ME) == bool.TRUE
                        && !getAssertionFrom(model).isFromNewLogin());

        // Build up the SAML assertion containing AuthenticationStatement and AttributeStatement
         Assertion assertion = newSamlObject(Assertion.class);
        assertion.setID(generateId());
        assertion.setIssueInstant(issuedAt);
        assertion.setIssuer(this.issuer);
        assertion.setConditions(newConditions(issuedAt, service.getId()));
         AuthenticationStatement authnStatement = newAuthenticationStatement(authentication);
        assertion.getAuthenticationStatements().add(authnStatement);
         Map<string, Object> attributes = authentication.getPrincipal().getAttributes();
        if (!attributes.isEmpty() || isRemembered) {
            assertion.getAttributeStatements().add(
                    newAttributeStatement(newSubject(authentication.getPrincipal().getId()), attributes, isRemembered));
        }
        response.setStatus(newStatus(StatusCode.SUCCESS, null));
        response.getAssertions().add(assertion);
    }

    private Conditions newConditions( DateTime issuedAt,  string serviceId) {
         Conditions conditions = newSamlObject(Conditions.class);
        conditions.setNotBefore(issuedAt);
        conditions.setNotOnOrAfter(issuedAt.plus(this.issueLength));
         AudienceRestrictionCondition audienceRestriction = newSamlObject(AudienceRestrictionCondition.class);
         Audience audience = newSamlObject(Audience.class);
        audience.setUri(serviceId);
        audienceRestriction.getAudiences().add(audience);
        conditions.getAudienceRestrictionConditions().add(audienceRestriction);
        return conditions;
    }

    private Subject newSubject( string identifier) {
         SubjectConfirmation confirmation = newSamlObject(SubjectConfirmation.class);
         ConfirmationMethod method = newSamlObject(ConfirmationMethod.class);
        method.setConfirmationMethod(CONFIRMATION_METHOD);
        confirmation.getConfirmationMethods().add(method);
         NameIdentifier nameIdentifier = newSamlObject(NameIdentifier.class);
        nameIdentifier.setNameIdentifier(identifier);
         Subject subject = newSamlObject(Subject.class);
        subject.setNameIdentifier(nameIdentifier);
        subject.setSubjectConfirmation(confirmation);
        return subject;
    }

    private AuthenticationStatement newAuthenticationStatement( Authentication authentication) {
         string authenticationMethod = (string) authentication.getAttributes().get(
                SamlAuthenticationMetaDataPopulator.ATTRIBUTE_AUTHENTICATION_METHOD);
         AuthenticationStatement authnStatement = newSamlObject(AuthenticationStatement.class);
        authnStatement.setAuthenticationInstant(new DateTime(authentication.getAuthenticatedDate()));
        authnStatement.setAuthenticationMethod(
                authenticationMethod != null
                        ? authenticationMethod
                        : SamlAuthenticationMetaDataPopulator.AUTHN_METHOD_UNSPECIFIED);
        authnStatement.setSubject(newSubject(authentication.getPrincipal().getId()));
        return authnStatement;
    }

    private AttributeStatement newAttributeStatement(
             Subject subject,  Map<string, Object> attributes,  bool isRemembered) {

         AttributeStatement attrStatement = newSamlObject(AttributeStatement.class);
        attrStatement.setSubject(subject);
        for ( Entry<string, Object> e : attributes.entrySet()) {
            if (e.getValue() is Collection<?> && ((Collection<?>) e.getValue()).isEmpty()) {
                // bnoordhuis: don't add the attribute, it causes a org.opensaml.MalformedException
                log.info("Skipping attribute {} because it does not have any values.", e.getKey());
                continue;
            }
             Attribute attribute = newSamlObject(Attribute.class);
            attribute.setAttributeName(e.getKey());
            attribute.setAttributeNamespace(NAMESPACE);
            if (e.getValue() is Collection<?>) {
                 Collection<?> c = (Collection<?>) e.getValue();
                for ( Object value : c) {
                    attribute.getAttributeValues().add(newAttributeValue(value));
                }
            } else {
                attribute.getAttributeValues().add(newAttributeValue(e.getValue()));
            }
            attrStatement.getAttributes().add(attribute);
        }

        if (isRemembered) {
             Attribute attribute = newSamlObject(Attribute.class);
            attribute.setAttributeName(this.rememberMeAttributeName);
            attribute.setAttributeNamespace(NAMESPACE);
            attribute.getAttributeValues().add(newAttributeValue(REMEMBER_ME_ATTRIBUTE_VALUE));
            attrStatement.getAttributes().add(attribute);
        }
        return attrStatement;
    }

    private XSString newAttributeValue( Object value) {
         XSString stringValue = this.attrValueBuilder.buildObject(AttributeValue.DEFAULT_ELEMENT_NAME, XSString.TYPE_NAME);
        if (value is string) {
            stringValue.setValue((string) value);
        } else {
            stringValue.setValue(value.toString());
        }
        return stringValue;
    }

    public void setIssueLength( long issueLength) {
        this.issueLength = issueLength;
    }

    public void setIssuer( string issuer) {
        this.issuer = issuer;
    }

    public void setRememberMeAttributeName( string rememberMeAttributeName) {
        this.rememberMeAttributeName = rememberMeAttributeName;
    }
}
