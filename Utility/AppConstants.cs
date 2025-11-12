namespace RI.Claim.Utility
{
    public static class AppConstants
    {
//mobileclaims.com.au/api/ClaimantPhoto/124 – where 124 is the ID of the claim record you wish to retrieve the claimant’s photo ID for.
//mobileclaims.com.au/api/RegularUserPhoto/1 – where 124 is the ID of the claim record you wish to retrieve the regular user’s photo ID for.
//mobileclaims.com.au/api/ImeiPaperwork/1 – where 124 is the ID of the claim record you wish to retrieve the IMEI paperwork for.

        public static string OnlineClaimUrl = "http://mobileclaims.com.au/api/Claims/";
        public static string ClaimantPhotoUrl = "http://mobileclaims.com.au/api/ClaimantPhoto/";
        public static string RegularUserPhotoUrl = "http://mobileclaims.com.au/api/RegularUserPhoto/";
        public static string ImeiPaperworkUrl = "http://mobileclaims.com.au/api/ImeiPaperwork/";

        public static string DocumentsFolder = @"\\riskinsuresvr\vodafone\Online_Cliams_Documents\";
    }
}
