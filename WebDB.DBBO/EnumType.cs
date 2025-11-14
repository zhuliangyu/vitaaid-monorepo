using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebDB.DBBO
{
    public enum eUNITTYPE
    {
        PRODUCT_CATEGORY = 100,
        ALLERGY_CATEGORY = 101,
        THERAPEUTIC_FOCUS = 102,
        PRACTICE_TYPE = 103,
        //SALES_REP = 104,
        EMAIL_SETTING = 201,
    }
    public enum eMEMBERTYPE
    {
        [Display(Name = "Healthcare Practitioner", Order = 1)]
        HEALTHCARE_PRACTITIONER = 1,
        [Display(Name = "Patient", Order = 2)]
        PATIENT = 2,
        [Display(Name = "Student [BINM]", Order = 3)]
        STUDENT_BINM = 3,
        [Display(Name = "Student [CCNM]", Order = 4)]
        STUDENT_CCNM = 4,
        [Display(Name = "Student [Others]", Order = 5)]
        STUDENT_Others = 5
    }
    public enum eMEMBERSTATUS
    {
        [Display(Name = "Inactive")]
        INACTIVE = 0,
        [Display(Name = "In review")]
        IN_REVIEW = 1,
        [Display(Name = "Rejected")]
        REJECTED = 2,
        [Display(Name = "Active")]
        ACTIVE = 9
    }
    //[Flags]
    public enum eWEBSITE
    {
        [Display(Name = "CA")]
        CA = 1,
        [Display(Name = "US")]
        US = 2,
        [Display(Name = "ALL")]
        ALL = 0xFF
    }

    public enum ePREFIX
    {
        [Display(Name = " ")]
        NONE = 9,
        [Display(Name = "Dr.")]
        DR = 0,
        [Display(Name = "Mr.")]
        MR  = 1,
        [Display(Name = "Ms.")]
        MS  = 2,
    }

}
