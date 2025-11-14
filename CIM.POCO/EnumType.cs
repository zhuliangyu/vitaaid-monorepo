using System.ComponentModel;

namespace POCO
{
  public enum ePOTENCYMETHOD
  {
    POTENCY = 1,
    PURITY = 2,
    CONCENTRATION = 3,
    STANDARDIZE = 4,
    NONE = 0
  }
  public enum eKEEPIN
  {
    NONE = 0,
    BAG,
    BLENDER
  }
  public enum eOPERATION
  {
    PREMIX = 1,
    PASS,
    DIVIDE
  }

  public enum eOPERAND
  {
    TEMPOPERAND = 1,
    RAWMATERIAL
  }

  public enum eVALUETYPE
  {
    STRING = 1,
    FLOAT,
    INT,
    DATE,
    TIME,
    DATETIME
  }
  public enum ePROCESSTYPE
  {
    BLENDING = 1500,
    MICRONUTRIENTS = 1530,
    MACRONUTRIENTS = 1540,
    ENCAPSULATION = 2500,
    POLISHING = 3500,
    INSPECTION = 4500,
    PACKAGING = 5500,
    QCQA = 6500,
    SHIPPING = 7500
  }
  public enum eFORMTYPE
  {
    BULK = 0,           // NONE
    CAPSULE = 1,        // INCLUDE BOTTLE, CAP, LABEL, CAPSULE, DESICCANT
    LIQUID = 2,         // INCLUDE BOTTLE, LABEL
    LIQUID_DROPPER = 3, // INCLUDE BOTTLE, CAP(Dropper), LABEL
    POWDER_BOTTLE = 4,  // INCLUDE BOTTLE, CAP, LABEL, DESICCANT, SCOOP
    POWDER_BAG = 5,     // INCLUDE LABEL, DESICCANT, SCOOP
  }
  public enum ePACKAGETYPE
  {
    CAPSULE = 1,
    LABEL,
    CAP,
    BOTTLE,
    DESICCANT,
    SCOOPS
  }
  public enum eUNITTYPE
  {
    WEIGHT = 1,
    VOLUME,
    CFU,
    POTENCY,
    COLOR,
    CAPSULE_SIZE,
    CAP_SIZE,
    CAP_TYPE,
    BOTTLE_SIZE,
    BOTTLE_TYPE,
    SCOOPS_SIZE,
    CAPSULE_TYPE,
    DESICCANT_TYPE,
    LABEL_TYPE,
    LABEL_BRAND,
    DESICCANT_SIZE,
    SCOOPS_TYPE,
    COUNTRY,
    PROVINCE,
    CURRENCY,
    PO_WEIGHT,
    PO_VOLUME,
    RMSTOCK,
    PMSTOCK,
    PRODUCTION_SUMMARY
  }
  public enum eLOTSTATUS2
  {
    INIT = 1,
    PREPARE,
    DISPENSE,
    SUSPEND,
    PRODUCE,
    SHIPPING,
    FINISH,
    ABORT
  }
  public enum eRMREQSTATUS
  {
    INIT = 1,
    RESERVE,
    DISPENSE,
    APPLY_END,
    ROLLBACK,
    SUSPEND,
    ABORT,

  }

  public enum eEQTYPE
  {
    GENERIC = 0,
    DISPENSE = 1,
    BLENDING,
    INSP,
    ENCAPSULATION,
    POLISHING,
    PACKAGING
  }
  public enum eEQDataItemType
  {
    RECIPE = 1,
    EDC = 2
  }

  public enum eVirtualLotStatus
  {
    ANALYSIS = 1,
    PURCHASERM,
    PRODUCTION,
    DELETED
  }

  public enum ePurchaseStatus
  {
    PROCESSING = 1,
    PURCHASED,
    SHIPPED,
    DELIVERED,
    RECEIVED
  }

  public enum eStabilityTestStatus
  {
    INIT = 0,
    T0,
    T12,
    T24,
    T36,
    T48,
    TESTING,
    REVIEW,
    CLOSED
  }

  public enum eSheetType
  {
    NORMAL = 0,
    AVG_ALARM = 1,
    VAR_ALARM = 2,
    PILOT_RUN = 8
  }
  public enum eProcessLogType
  {
    INFO = 0,
    WARNING,
    ERROR,
    FATAL,
    SYSTEM
  }
  public enum eREPROCESS
  {
    [Description("")]
    UNDEFINED = 0,
    [Description("MUST_REPROCESS")]
    MUST_REPROCESS = 1,
    [Description("MAY_NEED_TO_REPROCESS")]
    MAY_NEED_TO_REPROCESS = 2,
    [Description("NO_NEED_TO_REPROCESS")]
    NO_NEED_TO_REPROCESS = 3
  }
}
