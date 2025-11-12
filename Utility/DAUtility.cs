using RI.Claim.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace RI.Claim
{
    static class DAUtility
    {
        private static RiskVHAEntities db = new RiskVHAEntities();
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public enum PersonTitle
        {
            Mr = 1,
            Mrs = 2,
            Ms = 3,
            Miss = 4,
            Dr = 5
        }

        /// <summary>
        /// Copy online claims into tblClaim
        /// </summary>
        public static void CreateOnlineClaim()
        {
            List<tblClaim> _Claims = new List<tblClaim>();
            var _Claim = new tblClaim();
            var OC = GetOnLineClaims();

            

            OC.ForEach(c =>
            {
                if (!ExistOnlineClaim(c.ID))
                {
                    try
                    {

                        _Claim = new tblClaim()
                        {
                            OnLineClaimID = c.ID,
                            ClaimNumber = db.GetClaimNumber().FirstOrDefault().ToString(),
                            //Default values: Need to ask Kylie whether its need it or not
                            SchemeID = GetScheme(c.ProductName), //372,//Default value
                            RepairCost = 400,
                            TotalClaimCost = 400,
                            GST = (decimal)36.36,
                            TotalCostLessGST = (decimal)363.64,
                            ReplaceCost = 0,
                            FreightCost = 0,
                            AccLeatherCase = 0,
                            SimcardCost = 0,
                            UnauthorisedCalls = 0,
                            cla_ShortfallUpgradeCost = 0,
                            ReplaceVarianceCost = 0,
                            AccOther = 0,
                            CashSettleAmount = 0,
                            cla_InsuranceType = "Post Paid",
                            Status = "More Info",
                            AfterHoursFlag = false,
                            InformationConsentFlag = false,
                            PreviousClaimFlag = false,
                            CancelInsuranceFlag = false,
                            SimCardFlag = false,
                            cla_SimCardPreviousFlag = false,
                            PoliceReportFlag = false,

                            cla_UnattendedPublic = false,
                            cla_UnattendedIntentional = false,
                            cla_UnattendedVehicle = false,
                            GSTRegisteredFlag = false,
                            cla_AuthoriseExcess = false,

                            cla_LodgerAccountHolderNoEnglish = false,
                            cla_DoNotOrder = false,
                            cla_BPayRequired = false,
                            cla_ProofOfForcibleEntry = false,
                            cla_IMEIBlocked = false,
                            cla_ProofOfPurchase = false,
                            oUseSupplier = false,
                            //-------------------------------------------------------------

                            ClaimType = "Online Claims",
                            GivenName = c.Firstname,
                            FamilyName = c.Surname,
                            CustomerEmail = c.Email,
                            MobilePhoneNumber = c.InsuredMobileNumber,
                            IMEINumber = c.InsuredIMEISerial,
                            PoliceReportNumber = c.PoliceReferenceNumber,

                            ContactNumber = c.DaytimeContactNo,
                            ContactNumber2 = c.AfterHoursContactNo,

                            PostalSuburb = c.Suburb,
                            PostalPostcode = c.Postcode,
                            cla_PostalState = c.StateAbbv,//c.State
                            PostalAddress1 = c.AddressLine1,
                            PostalAddress2 = c.AddressLine2,

                            cla_DeliverPerson = c.FullName,
                            cla_DeliveryState = c.StateAbbv,
                            DeliverySuburb = c.Suburb,
                            DeliveryPostcode = c.Postcode,
                            DeliveryAddress1 = c.AddressLine1,
                            DeliveryAddress2 = c.AddressLine2,

                            cla_LodgerFirstName = c.RegularUserTitle + " " + c.RegularUserFirstname,
                            cla_LodgerLastName = c.RegularUserSurname,
                            cla_LodgerRelationship = c.RegularUserRelationship,
                            cla_LodgerHasAuthority = c.RegularUserAuthorised,
                            DamageType = BuildDamageType(c),

                            DiaryEntry = DiaryEntryEntry(c.ClaimType),

                            LossType = c.ClaimType
                        };

                        if (c.ClaimType.ToLower() == "damage")
                        {
                            _Claim.DiaryDate = GetNextWorkingDay(DateTime.Today.AddDays(27));
                            _Claim.CaseManager = "Damage Team";
                        }
                        //For lost/stolen claims
                        else
                        {
                            _Claim.DiaryDate = GetNextWorkingDay(DateTime.Today.AddDays(0));
                        }


                        _Claim.Title = GetTitle(c.Title);
                        _Claim.DateAdded = ConvertDate(c.CreateDate);
                        _Claim.LodgedDate = DateTime.Now;// : ConvertDate(c.ClaimDate);
                        _Claim.LossDate = ConvertDate(c.DateOfIncident);

                        _Claim.WarningNote = "";
                        if (ConvertDate(c.DateOfBirth) != null)
                            _Claim.WarningNote = "Date of Birth: " + c.DateOfBirth.Value.ToString("dd/MM/yyyy") + Environment.NewLine;
                        if (!string.IsNullOrEmpty(c.DriverPassportNumber))
                            _Claim.WarningNote = _Claim.WarningNote + "Driver Passport Number: " + c.DriverPassportNumber + Environment.NewLine;
                        if (!string.IsNullOrEmpty(c.TypeOfDamage))
                            _Claim.WarningNote = _Claim.WarningNote + "Damage type: " + c.TypeOfDamage + Environment.NewLine;

                        if (c.OtherClaimant == true)
                            _Claim.WarningNote = _Claim.WarningNote + "Have other claimant";


                        if (!string.IsNullOrEmpty(_Claim.PoliceReportNumber))
                            _Claim.PoliceReportFlag = true;

                        if (!string.IsNullOrEmpty(c.AfterHoursContactNo))
                            _Claim.AfterHoursFlag = true;

                        //Need more info; waiting for Paul's reply
                        _Claim.CurrentPhoneID = FindPhone(c.DeviceMakeModelColour);

                        db.tblClaims.Add(_Claim);

                        //Add Cliam note - (Police reference number) in tblClaimNote table
                        if (ConvertDate(c.ClaimDate) != null)
                        {
                            var _Note = new tblClaimNote()
                            {
                                ClaimNumber = _Claim.ClaimNumber,
                                NoteType = "Comments",
                                Note = "ONLINE CLAIM FORM SUBMITTED " + c.ClaimDate.Value.ToString("dd/MM/yyyy"),
                                Delete = false,
                                DateAdded = DateTime.Now
                            };

                            db.tblClaimNotes.Add(_Note);
                        }

                        //Mark pulling completed
                        c.NeedToPull = false;
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        db = new RiskVHAEntities();
                        log.Error(string.Format("Can't pull this online claim: {0}", ex.ToString()));
                        log.Info(JsonConvert.SerializeObject(c));
                    }
                }
                else
                {
                    c.NeedToPull = false;

                    db.OnlineClaims.Add(c);
                    db.Entry(c).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();                    
                }
            });
        }






        private static int GetScheme(string schemename)
        {
            var _Scheme = db.tblSchemes.Where(s => s.SchemeName == schemename).FirstOrDefault();
            return _Scheme == null ? 372 : _Scheme.SchemeID;
        }


        private static string DiaryEntryEntry(string claimType)
        {
            var diaryEntry = "";
            if (claimType.ToLower() == "damage")
            {
                diaryEntry = "WTG HANDSET/DOCS - ONLINE"; //WTG HANDSET/DOC’S
            }
            else
            {
                diaryEntry = "WTG CM L/S";
            }

            return diaryEntry;// + Environment.NewLine + "ONLINE CLAIM FORM SUBMITTED " + DateTime.Today.ToString("dd/MM/yyyy");
        }

        private static bool IsWeekend(DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday
                || date.DayOfWeek == DayOfWeek.Sunday;
        }


        private static DateTime GetNextWorkingDay(DateTime date)
        {
            do
            {
                date = date.AddDays(1);
            } while (IsWeekend(date));
            return date;
        }


        private static string GetTitle(string _title)
        {
            switch (_title)
            {
                case "1":
                    return PersonTitle.Mr.ToString();
                case "2":
                    return PersonTitle.Mrs.ToString();
                case "3":
                    return PersonTitle.Ms.ToString();
                case "4":
                    return PersonTitle.Miss.ToString();
                case "5":
                    return PersonTitle.Dr.ToString();

            }
            return PersonTitle.Mr.ToString();
        }

        private static int FindPhone(string description)
        {
            //var Phone = db.tblPhones.Where(p => p.Description.ToLower() == description.ToLower()).FirstOrDefault();
                                   
            var PhoneList = db.tblPhones.ToList();
            PhoneList.ForEach(p => {
                p.Make = p.Make == null ? "" : p.Make.Trim();
                p.Model = p.Model == null ? "" : p.Model.Trim();
                p.ImportColour = p.ImportColour == null ? "" : p.ImportColour.Trim();
                p.Description = string.Format("{0} {1} {2}", p.Make, p.Model, p.ImportColour);                
            });
            
            var Phone = PhoneList.Where(p => p.Description.ToLower() == description.ToLower()).FirstOrDefault();

            return Phone == null ? 0 : Phone.PhoneID;            
        }

        private static Nullable<DateTime> ConvertDate(DateTime? dt)
        {
            try
            {
                if (dt == DateTime.Parse("1/01/0001 12:00:00 AM"))
                    return null;
                else

             return DateTime.Parse(dt.ToString());
            }
            catch
            {
                return null;
            }
        }


        private static string BuildDamageType(OnlineClaim c)
        {
            string _damage = string.Empty;

            try
            {
                _damage = ConvertDate(c.DateOfIncident) != null ? "Date of incident: " + c.DateOfIncident.Value.ToString("dd/MM/yyyy") + Environment.NewLine : _damage;
                
                if (c.ClaimType == "Damage")
                {
                    _damage = _damage + "Is there Physical or Liquid damage to the device? " + c.TypeOfDamage
                    + Environment.NewLine + "Does the device power on? " + BooleanToString(c.PowerOnWorking)
                    + Environment.NewLine + "Is the display working? " + BooleanToString(c.DisplayWorking)
                    + Environment.NewLine + "Is the glass screen cracked or damaged? " + BooleanToString(c.ScreenDamaged)
                    + Environment.NewLine + "Is the touch working on the screen? " + BooleanToString(c.TouchWorking)
                    + Environment.NewLine + "Is the back glass cracked or damaged? " + BooleanToString(c.BackGlassDamaged)
                    + Environment.NewLine + "Is the rear facing camera damaged? " + BooleanToString(c.RearCameraDamaged)
                    + Environment.NewLine + "Are the earphones and speakers working? " + BooleanToString(c.EarphonesSpeakersWorking)
                    + Environment.NewLine + "Is the charging port damaged? " + BooleanToString(c.ChargingPortDamaged)
                    + Environment.NewLine + "Has the device sustained liquid damage other than this incident? " + BooleanToString(c.PreviousLiquidDamage)
                    + Environment.NewLine + "What Mobile Number was in the Device at the time of the Damage? " + c.MobileNoInDeviceAtTimeOfDamage.ToStringEx();
                }

                else if (c.ClaimType == "LostStolen")
                {
                    //Lost or Stolen
                    string lastTimeDeviceSeen = ConvertDate(c.LastTimeDeviceSeen) != null ? c.LastTimeDeviceSeen.Value.ToString("dd/MM/yyyy HH:mm:ss") : "";
                    string dateRealisedMissing = ConvertDate(c.DateRealisedMissing) != null ? c.DateRealisedMissing.Value.ToString("dd/MM/yyyy HH:mm:ss") : "";

                    _damage = _damage + "Was the device lost or stolen within Australia or whilst Overseas? " + c.AustraliaOrOverseas.ToStringEx()
                        + Environment.NewLine + "Before your loss/theft when was the last time and date you saw your device? " + lastTimeDeviceSeen.ToStringEx()
                    + Environment.NewLine + "Where exactly was the device the last time you saw it? " + c.WhereWasDevice.ToStringEx()
                    + Environment.NewLine + "What was your location the last time you saw the device? " + c.IncidentLocation.ToStringEx()
                    + Environment.NewLine + "What was the exact time and date you realised your device was missing? " + dateRealisedMissing
                    + Environment.NewLine + "What was your location when you realised that the device was missing? " + c.LocationRealisedMissing
                    + Environment.NewLine + "Please provide a specific and detailed explanation of the events or actions that took place between the last time you saw the device and when you realised the device was missing. " + c.DetailedExplanation.ToStringEx()
                    + Environment.NewLine + "What was the Mobile Number for the SIM in the Device at the time of the loss/theft? " + c.MobileNoWhenMissing.ToStringEx()
                    + Environment.NewLine + "What telco network does the this Mobile Number work on? " + c.TelcoNetwork.ToStringEx()
                    + Environment.NewLine + "Since the loss/theft have you had your SIM card replaced? " + BooleanToString(c.HasSIMCardBeenReplaced)
                    + Environment.NewLine + "A Police Report Incident or Event Number is required for all lost/stolen claims, have you obtained this from the police? " + BooleanToString(!string.IsNullOrEmpty(c.PoliceReferenceNumber))
                    ;
                }

                return _damage;

            }
            catch (Exception ex)
            {
                log.Info(string.Format("Damage description conversion error. {0}", ex.ToString()));
                log.Info(JsonConvert.SerializeObject(c));
            }
            return _damage;
        }        

        private static string BooleanToString(bool? data)
        {
            switch (data)
            {
                case true:
                    return "Yes";
                case false:
                    return "No";
                default:
                    return "";
            }        
        }


        public static string ToStringEx(this String str)
        {
            return string.IsNullOrEmpty(str) == true ? "" : str;
        }


     
        /// <summary>
        /// Pull online claims by APIs and Add if its new otherwise update
        /// </summary>
        /// <param name="_OnLineClaims"></param>
        public static void PullOnLineClaim(List<OnlineClaim> _OnLineClaims)
        {
            _OnLineClaims.ForEach(oc =>
            {
                try
                {
                    if (db.OnlineClaims.Where(c => c.ID == oc.ID).FirstOrDefault() == null)
                    {                       
                        db.OnlineClaims.Add(oc);
                        db.SaveChanges();
                    }
                    else
                    {
                        log.Info("This claim is already exist.");
                        log.Info(JsonConvert.SerializeObject(oc));
                    }
                }
                catch (Exception ex)
                {
                    db=new RiskVHAEntities();
                    log.Error(string.Format("Online claim insert error: {0}", ex.ToString()));
                    log.Info(JsonConvert.SerializeObject(oc));
                }
            });
        }

        public static List<OnlineClaim> GetOnLineClaims()
        {            
            return db.OnlineClaims.Where(oc => oc.NeedToPull == true).ToList();
        }
        /// <summary>
        /// Is already pulled??
        /// </summary>
        /// <returns></returns>
        public static bool ExistOnlineClaim(int onlineClaimId)
        {                 
           return db.tblClaims.Where(oc => oc.OnLineClaimID == onlineClaimId).FirstOrDefault() != null;
        }

        public static int LastOnlineClaimId()
        {
            var Claim=db.OnlineClaims.OrderByDescending(o => o.ID).FirstOrDefault();
            return Claim == null ? 1 : Claim.ID;             
        }

        public static string AttachmentName(string fileName)
        {
            //D:\\Inetpub\\vhosts\\mobileclaims.com.au\\httpdocs\\Data\\4621f661 - 35cf - 4f2a - b559 - 38c1ab326d82_alien.jpeg
            var names = fileName.Split('\\');
            return names.Length == 0 ? "" : names[names.Length - 1];
        }
    }
}
