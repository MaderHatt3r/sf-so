﻿// ***********************************************************************
// Assembly         : InternalLibrary
// Author           : CTDragon
// Created          : 06-13-2013
//
// Last Modified By : CTDragon
// Last Modified On : 06-13-2013
// ***********************************************************************
// <copyright file="AuthenticationManager.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Google.Apis.Drive.v2;
using DotNetOpenAuth.OAuth2;
using Google.Apis.Authentication.OAuth2.DotNetOpenAuth;
using InternalLibrary.Data;
using System.Security.Cryptography;
using System.Diagnostics;
using InternalLibrary.Forms;
using Google.Apis.Util;





namespace InternalLibrary.Model
{
    /// <summary>
    /// Class AuthenticationManager
    /// </summary>
    public static class AuthenticationManager
    {
        /// <summary>
        /// Gets the authorization.
        /// </summary>
        /// <param name="arg">The arg.</param>
        /// <returns>IAuthorizationState.</returns>
        /// <exception cref="System.OperationCanceledException">Authorization canceled by user</exception>
        public static IAuthorizationState GetAuthorization(NativeApplicationClient arg)
        {
            string storage = GlobalApplicationOptions.SERVICE_PATH + GlobalApplicationOptions.SERVICE_FILE_NAME;
            string key = GlobalApplicationOptions.KEY;
            System.IO.Directory.CreateDirectory(GlobalApplicationOptions.SERVICE_PATH);

            // Check if there is a cached refresh token available.
            IAuthorizationState state = GetCachedRefreshToken(storage, key);
            if (state != null)
            {
                try
                {
                    arg.RefreshToken(state);
                    return state; // Yes - we are done.
                }
                catch (DotNetOpenAuth.Messaging.ProtocolException ex)
                {
                    //TODO: Move the message box to the GUI
                    System.Windows.Forms.MessageBox.Show("Using existing refresh token failed: " + ex.Message);
                }
            }

            // Get the auth URL:
            state = new AuthorizationState(new[] { DriveService.Scopes.Drive.GetStringValue() });
            state.Callback = new Uri(NativeApplicationClient.OutOfBandCallbackUrl);
            Uri authUri = arg.RequestUserAuthorization(state);

            // Request authorization from the user (by opening a browser window):
            Process.Start(authUri.ToString());

            // Retrieve authorization code from the user
            string authCode = "";
            if (AuthenticationForm.InputBox("  Authorization Code  ", "Authorization Code:", ref authCode) == System.Windows.Forms.DialogResult.OK)
            {
                // Return the access token by using the authorization code:
                state = arg.ProcessUserAuthorization(authCode, state);
                SetCachedRefreshToken(storage, key, state);
                return state;
            }
            else
            {
                throw new OperationCanceledException("Authorization canceled by user");
            }
        }

        /// <summary>
        /// Returns a cached refresh token for this application, or null if unavailable.
        /// </summary>
        /// <param name="storageName">The file name (without extension) used for storage.</param>
        /// <param name="key">The key to decrypt the data with.</param>
        /// <returns>The authorization state containing a Refresh Token, or null if unavailable</returns>
        private static AuthorizationState GetCachedRefreshToken(string storageName,
                                                               string key)
        {
            string file = storageName + ".auth";
            byte[] contents = null;
            if (System.IO.File.Exists(file))
            {
                contents = System.IO.File.ReadAllBytes(file);
            }

            if (contents == null)
            {
                return null; // No cached token available.
            }

            byte[] salt = Encoding.Unicode.GetBytes("5" + key);
            byte[] decrypted = ProtectedData.Unprotect(contents, salt, DataProtectionScope.CurrentUser);
            string[] content = Encoding.Unicode.GetString(decrypted).Split(new[] { "\r\n" }, StringSplitOptions.None);

            // Create the authorization state.
            //IAuthorizationState state = new AuthorizationState(new[] { DriveService.Scopes.Drive.GetStringValue() });
            string[] scopes = content[0].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string refreshToken = content[1];
            return new AuthorizationState(scopes) { RefreshToken = refreshToken };
        }

        /// <summary>
        /// Saves a refresh token to the specified storage name, and encrypts it using the specified key.
        /// </summary>
        /// <param name="storageName">Name of the storage.</param>
        /// <param name="key">The key.</param>
        /// <param name="state">The state.</param>
        private static void SetCachedRefreshToken(string storageName,
                                                 string key,
                                                 IAuthorizationState state)
        {
            // Create the file content.
            string scopes = state.Scope.Aggregate("", (left, append) => left + " " + append);
            string content = scopes + "\r\n" + state.RefreshToken;

            // Encrypt it.
            byte[] salt = Encoding.Unicode.GetBytes("5" + key);
            byte[] encrypted = ProtectedData.Protect(
                Encoding.Unicode.GetBytes(content), salt, DataProtectionScope.CurrentUser);

            // Save the data to the auth file.
            string file = storageName + ".auth";
            System.IO.File.WriteAllBytes(file, encrypted);
        }

    }
}
