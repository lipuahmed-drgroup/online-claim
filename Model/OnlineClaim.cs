using System;
using System.Collections.Generic;

namespace RI.Claim.Model
{  
    public class AdditionalClaimant
    {
        public string title { get; set; }
        public string firstname { get; set; }
        public string surname { get; set; }
        public string relationship { get; set; }
    }

    //Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class DamageDetails
    {
        public string howDamageOccurred { get; set; }
        public string descriptionOfDamage { get; set; }
        public bool devicePowerOn { get; set; }
        public bool previousLiquidDamage { get; set; }
        public string mobileNoInDevice { get; set; }
        public string howTheftLossOccurred { get; set; }
        public string dateLastSeen { get; set; }
        public string timeWentMissing { get; set; }
        public bool simCardReplaced { get; set; }
        public string simReplacementDate { get; set; }
        public string simReplacementTime { get; set; }
        public bool imeiBlockRequested { get; set; }
        public bool anythingElseLostStolen { get; set; }
        public string descriptionOfAnythingElseLostStolen { get; set; }
        public string policeReferenceNo { get; set; }
        public string policeStationReportedAt { get; set; }
        public string policeReportDate { get; set; }
    }

    public class TheftDetails
    {

    }

    public class MyArray
    {
        public int ID { get; set; }
        public string ClaimType { get; set; }
        public int Title { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string DriverPassportNumber { get; set; }
        public string DaytimeContactNo { get; set; }
        public string AfterHoursContactNo { get; set; }
        public string Email { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Suburb { get; set; }
        public string Postcode { get; set; }
        public int State { get; set; }
        public bool OtherClaimant { get; set; }
        public int RegularUserTitle { get; set; }
        public string RegularUserFirstname { get; set; }
        public string RegularUserSurname { get; set; }
        public DateTime RegularUserDateOfBirth { get; set; }
        public string RegularUserRelationship { get; set; }
        public bool RegularUserAuthorised { get; set; }
        public string RegularUserDriverPassportNumber { get; set; }
        public string InsuredMobileNumber { get; set; }
        public string InsuredIMEISerial { get; set; }
        public bool WarrantyRepair { get; set; }
        public string DeviceMake { get; set; }
        public string DeviceModelColour { get; set; }
        public DateTime DateOfIncident { get; set; }
        public string IncidentLocation { get; set; }
        public string AustraliaOrOverseas { get; set; }
        public DateTime LastTimeDeviceSeen { get; set; }
        public string WhereWasDevice { get; set; }
        public string DetailedExplanation { get; set; }
        public string MobileNoWhenMissing { get; set; }
        public string TelcoNetwork { get; set; }
        public bool HasSIMCardBeenReplaced { get; set; }
        public DateTime SIMReplacedDateTime { get; set; }
        public string PoliceReferenceNumber { get; set; }
        public string TypeOfDamage { get; set; }
        public bool PowerOnWorking { get; set; }
        public bool DisplayWorking { get; set; }
        public bool ScreenDamaged { get; set; }
        public bool TouchWorking { get; set; }
        public bool BackGlassDamaged { get; set; }
        public bool RearCameraDamaged { get; set; }
        public bool EarphonesSpeakersWorking { get; set; }
        public bool ChargingPortDamaged { get; set; }
        public bool PreviousLiquidDamage { get; set; }
        public string MobileNoInDeviceAtTimeOfDamage { get; set; }
        public bool DeclarationOfTruth { get; set; }
        public string ClaimantPhoto { get; set; }
        public string RegularUserPhoto { get; set; }
        public string IMEIAttachedFilename { get; set; }
        public string FullName { get; set; }
        public string StateAbbv { get; set; }

    }

    public class Root
    {
        public List<MyArray> MyArray { get; set; }

    }


}




