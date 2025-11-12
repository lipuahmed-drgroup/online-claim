using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using RI.Claim.Utility;
using RI.Claim.Entity;


namespace RI.Claim
{
    class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
          
        static void Main(string[] args)
        {
            
            log.Info("App started");
            var _Client = new WebClient();            
           //_Client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            _Client.Headers.Add("API_KEY", "BF143341-54F4-47BF-80E7-A7FE2003C4DA");
            var Id = DAUtility.LastOnlineClaimId();           
            var data = _Client.DownloadString(AppConstants.OnlineClaimUrl + Id);
            var _OnlineClaims = JsonConvert.DeserializeObject<List<OnlineClaim>>(data);
            var _Claims = new List<tblClaim>();

            //"ProductName": "$10 Keep Talking",

            //Validate
            _OnlineClaims.ForEach(c =>
            {
               // c.PayLoad = JsonConvert.SerializeObject(c);
                try
                {
                    //Documents downloads
                    if (!string.IsNullOrEmpty(c.ClaimantPhoto))
                    {
                        c.ClaimantPhoto = DAUtility.AttachmentName(c.ClaimantPhoto);
                        _Client.DownloadFile(AppConstants.ClaimantPhotoUrl + c.ID, AppConstants.DocumentsFolder + c.ClaimantPhoto);                        
                    }

                    if (!string.IsNullOrEmpty(c.RegularUserPhoto))
                    {
                        c.RegularUserPhoto = DAUtility.AttachmentName(c.RegularUserPhoto);
                        _Client.DownloadFile(AppConstants.RegularUserPhotoUrl + c.ID, AppConstants.DocumentsFolder + c.RegularUserPhoto);
                    }

                    if (!string.IsNullOrEmpty(c.IMEIAttachedFilename))
                    {
                        c.IMEIAttachedFilename = DAUtility.AttachmentName(c.IMEIAttachedFilename);
                        _Client.DownloadFile(AppConstants.ImeiPaperworkUrl + c.ID, AppConstants.DocumentsFolder + c.IMEIAttachedFilename);
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Could not download one of the attachments for this claim. Error: " + ex.ToString());
                    log.Info(JsonConvert.SerializeObject(c));
                }
                
                c.NeedToPull = true;
                c.CreateDate = DateTime.Now;
            });

            DAUtility.PullOnLineClaim(_OnlineClaims);

            DAUtility.CreateOnlineClaim();
        }
    }
}
