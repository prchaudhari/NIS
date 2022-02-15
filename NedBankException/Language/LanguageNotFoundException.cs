﻿// <copyright file="ConnectionStringNotFoundException.cs" company="Websym Solutions Pvt. Ltd.">
// Copyright (c) 2018 Websym Solutions Pvt Ltd.
// </copyright>
//------------------------------------------------------------------------------

namespace NedBankException
{
    #region references

    using System;

    #endregion

    /// <summary>
    /// This class implements role store not accessible exception which will
    /// be raised when an attempt is made to access repository with wrong credentials.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class LanguageNotFoundException : Exception
    {
        #region Private Members

        /// <summary>
        /// The tenant code
        /// </summary>
        private string tenantCode = string.Empty;

        #endregion

        #region Contructor

        /// <summary>
        /// Initializing instance of class.
        /// </summary>
        /// <param name="tenantCode">The tenant code.</param>
        public LanguageNotFoundException(string tenantCode)
        {
            this.tenantCode = tenantCode;
        }

        #endregion

        #region Public Members

        /// <summary>
        /// This method overrides the exception message.
        /// </summary>
        public override string Message
        {
            get
            {
                return ExceptionConstant.COMMON_EXCEPTION_SECTION + "~" + ExceptionConstant.INVALID_CONNECTIONSTRING_EXCEPTION;
            }
        }

        #endregion
    }
}
