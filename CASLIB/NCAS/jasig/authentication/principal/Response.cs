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
////package org.jasig.cas.authentication.principal;

////import java.net.URLEncoder;
////import java.util.Map;
////import java.util.regex.Matcher;
////import java.util.regex.Pattern;

////import org.slf4j.Logger;
////import org.slf4j.LoggerFactory;

/**
 * Encapsulates a Response to send back for a particular service.
 * 
 * @author Scott Battaglia
 * @author Arnaud Lesueur
 * @version $Revision: 1.1 $ $Date: 2005/08/19 18:27:17 $
 * @since 3.1
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace NCAS.jasig.authentication.principal
{
    public sealed class Response
    {
        /** Pattern to detect unprintable ASCII characters. */
        private static readonly Regex NON_PRINTABLE = new Regex("[\\x00-\\x19\\x7F]+");

        /** Log instance. */
        //protected static  Logger LOG = LoggerFactory.getLogger(Response.);

        public enum ResponseType
        {
            POST, REDIRECT
        }

        private ResponseType responseType;

        private string url;

        private Dictionary<string, string> attributes;

        protected Response(ResponseType responseType, string url, Dictionary<string, string> attributes)
        {
            this.responseType = responseType;
            this.url = url;
            this.attributes = attributes;
        }

        public static Response getPostResponse(string url, Dictionary<string, string> attributes)
        {
            return new Response(ResponseType.POST, url, attributes);
        }

        public static Response getRedirectResponse(string url, Dictionary<string, string> parameters)
        {
            StringBuilder builder = new StringBuilder(parameters.Count * 40 + 100);
            bool isFirst = true;
            string[] fragmentSplit = sanitizeUrl(url).Split("#".ToCharArray());

            builder.Append(fragmentSplit[0]);



            foreach (var entry in parameters)
            {
                if (entry.Value != null)
                {
                    if (isFirst)
                    {
                        builder.Append(url.Contains("?") ? "&" : "?");
                        isFirst = false;
                    }
                    else
                    {
                        builder.Append("&");
                    }
                    builder.Append(entry.Key);
                    builder.Append("=");

                    try
                    {
                        builder.Append(Dev.Comm.Core.Utils.MockUrlCode.UrlEncode(entry.Value/*, "UTF-8"*/));
                    }
                    catch (Exception e)
                    {
                        builder.Append(entry.Value);
                    }
                }
            }

            if (fragmentSplit.Length > 1)
            {
                builder.Append("#");
                builder.Append(fragmentSplit[1]);
            }

            return new Response(ResponseType.REDIRECT, builder.ToString(), parameters);
        }

        public Dictionary<string, string> getAttributes()
        {
            return this.attributes;
        }

        public ResponseType getResponseType()
        {
            return this.responseType;
        }

        public string getUrl()
        {
            return this.url;
        }

        /**
     * Sanitize a URL provided by a relying party by normalizing non-printable
     * ASCII character sequences into spaces.  This functionality protects
     * against CRLF attacks and other similar attacks using invisible characters
     * that could be abused to trick user agents.
     * 
     * @param  url  URL to sanitize.
     * 
     * @return  Sanitized URL string.
     */
        private static string sanitizeUrl(string url)
        {
            //var m = NON_PRINTABLE.Match(url);
            //StringBuilder sb = new StringBuilder(url.Length);
            //bool hasNonPrintable = false;
            //while ( (var m = m.NextMatch())!= null)
            //{
            //    m.(sb, " ");
            //    hasNonPrintable = true;
            //}
            //m.appendTail(sb);
            //if (hasNonPrintable)
            //{
            //    LOG.warn("The following redirect URL has been sanitized and may be sign of attack:\n" + url);
            //}

            return Dev.Comm.RegexHelper.PregReplace(url, "[\\x00-\\x19\\x7F]+", " ");

            //return sb.toString();
        }
    }
}
