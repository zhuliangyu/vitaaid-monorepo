update StabilityTestData
set TestSpec = '150 mg
90 ~ 125%'
from StabilityTestData std join StabilityForm sf on std.StabilityFormID = sf.ID
where sf.Code = 'VA-035' and std.GroupName = 'Critical Ingredient(s)/Dosage Unit:' and std.TestName = 'Magnesium'