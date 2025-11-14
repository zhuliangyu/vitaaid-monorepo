update PurchaseOrder
set DataStatus = 'A'
GO

update PackagePurchaseOrder
set DataStatus = 'A'
GO

update PackagePurchaseReq
set DataStatus  = 'A'
GO

update PurchaseRMReqInfo
set DataStatus  = 'A'
GO

/*
update Lot
set DataStatus = 'A'
GO

update VirtualLotInfo
set DataStatus = 'A'
GO

update VirtualLotInfo
set VirtualLotInfo.CreatedDate = lot.CreatedDate, VirtualLotInfo.UpdatedDate = lot.UpdatedDate,
	VirtualLotInfo.CreatedID = lot.CreatedID, VirtualLotInfo.UpdatedID = lot.UpdatedID,
	VirtualLotInfo.Sequence = lot.Sequence
from lot
where lot.id = VirtualLotInfo.oBindFabLot
GO

update FabPackageReq
set DataStatus = 'A'
GO

update ProductPackageReq
set DataStatus = 'A'
GO

update ProductPackageReq
 set CategoryCode = substring(PackageCode, 1, 6)
GO

update FabPackageReq
 set CategoryCode = substring(PackageCode, 1, 6)
GO

update FabProduction
set DataStatus = 'A', CreatedID = ModifiedID
GO

update FabPackageReq
set Qty = servings
GO

update Production
set DataStatus = 'A'
GO

update FabRawMaterialReq
set DataStatus = 'A'
GO

Update UnitType
set DataStatus = 'A', CreatedDate = GETDATE(), CreatedID = '', UpdatedDate = GETDATE(), UpdatedID = ''
GO

update RawMaterialDetail
set DataStatus = 'A'
GO

update MESRawMaterial
set DataStatus = 'A'
GO

/****** Script for eFORMTYPE  ******/
update Production
set FormType = ProductionFormType_v.tval
from ProductionFormType_v
where Production.id = ProductionFormType_v.id
GO

update Production
set FormType = xx.FormType
from (
select distinct x.id, dbo.Production.FormType
from Production right join (
select ID, Code,Name, isActive , FormType
from Production
where FormType is null) x on x.code = dbo.Production.Code and x.id <> dbo.Production.id 
where  not (dbo.Production.FormType is null)) xx
where xx.ID = Production.ID
GO

update Production
set FormType = 0
where FormType is null and Code like 'R%'
GO

update Production
set FormType = 1
where FormType is null and Code in ('VA-138', 'VA-157', 'VA-161')
GO


update VirtualLotInfo
set VirtualLotNo = lot.no
from lot 
where VirtualLotInfo.oBindFabLot = lot.id and not (lot.No like 'V%')
*/