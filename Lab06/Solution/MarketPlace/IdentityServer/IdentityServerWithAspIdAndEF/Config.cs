// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityServerWithAspNetIdentity {
    public class Config {
        // scopes define the resources in your system
        public static IEnumerable<IdentityResource> GetIdentityResources() {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
            };
        }

        public static IEnumerable<ApiResource> GetApiResources() {
            return new List<ApiResource> {
                new ApiResource("marketplaceapi", "MarketPlace API") {
                    // include the following using claims in access token (in addition to subject id)
                    // requires using IdentityModel;
                    UserClaims = { JwtClaimTypes.Name }
                }
            };
        }

        // clients want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients() {
            // client credentials client
            return new List<Client> {
                new Client {
                    ClientId = "marketplacejs",
                    ClientName = "MarketPlace JavaScript Client",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris =           { "http://localhost:5001/callback" },
                    PostLogoutRedirectUris = { "http://localhost:5001/index.html" },
                    AllowedCorsOrigins =     { "http://localhost:5001" },

                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Email,
                        "marketplaceapi"
                    }
                }
            };
        }
    }
}