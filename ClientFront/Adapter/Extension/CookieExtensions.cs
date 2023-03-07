// using Microsoft.AspNetCore.Http;
// using RestSharp;
// using System;
// using System.Net;
// using HttpResponse = Microsoft.AspNetCore.Http.HttpResponse;
//
// namespace Adapter.Extension
// {
//     /// <summary>
//     /// Class provides methods for containing installationId in cookies, when event sents from browser version.
//     /// </summary>
//     public static class CookieExtensions
//     {
//         
//
//         public static void InjectSessionInfoCookie(HttpResponse currentResponse, RestResponse externalResponse, HttpRequest request)
//         {
//             if (AppSettingsProvider.IsAnalyticEnabled && externalResponse.Cookies != null && externalResponse.Cookies.Count > 0)
//             {
//                 foreach (Cookie cookie in externalResponse.Cookies)
//                 {
//                     if (!request.Cookies.TryGetValue(AppSettingsProvider.AnalyticCookieUniqueidentifierName, out _))
//                     {
//                         AppendCookie(currentResponse, cookie);
//                     }
//                 }
//             }
//         }
//
//         private static void AppendCookie(HttpResponse currentResponse, Cookie externalCookie)
//         {
//             currentResponse.Cookies.Append(AppSettingsProvider.AnalyticCookieUniqueidentifierName, externalCookie.Value, new CookieOptions()
//             {
//                 Expires = externalCookie.Expires,
//                 Domain = externalCookie.Domain,
//                 Path = externalCookie.Path
//             });
//         }
//     }
// }
