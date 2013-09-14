// ***********************************************************************
// Assembly         : InternalLibrary
// Author           : CTDragon
// Created          : 09-07-2013
//
// Last Modified By : CTDragon
// Last Modified On : 09-07-2013
// ***********************************************************************
// <copyright file="ServiceBuilder.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.Apis.Authentication.OAuth2;
using Google.Apis.Authentication.OAuth2.DotNetOpenAuth;
using Google.Apis.Drive.v2;
using Google.Apis.Services;
using InternalLibrary.Data;

namespace InternalLibrary.Model.Builder
{
    /// <summary>
    /// Class ServiceBuilder.
    /// </summary>
    public static class ServiceBuilder
    {

        /// <summary>
        /// Builds the service.
        /// </summary>
        /// <returns>DriveService.</returns>
        public static DriveService BuildService()
        {
            // Register the authenticator and create the service
            var provider = new NativeApplicationClient(GoogleAuthenticationServer.Description, GlobalApplicationOptions.CLIENT_ID, GlobalApplicationOptions.CLIENT_SECRET);
            var auth = new OAuth2Authenticator<NativeApplicationClient>(provider, AuthenticationManager.GetAuthorization);
            var service = new DriveService(new BaseClientService.Initializer()
            {
                Authenticator = auth
            });

            return service;
        }
    }
}
