update FabRawMaterialApply
set CreatedID = MUser.Name
from MUser
where MUser.id = FabRawMaterialApply.CreatedUser
update FabRawMaterialApply
set UpdatedID = MUser.Name
from MUser
where MUser.id = FabRawMaterialApply.UpdatedUser;

update FabRawMaterialReq
set CreatedID = MUser.Name
from MUser
where MUser.id = FabRawMaterialReq.CreatedUser;
update FabRawMaterialReq
set UpdatedID = MUser.Name
from MUser
where MUser.id = FabRawMaterialReq.UpdatedUser;

/****** FabPackageReq  ******/
update FabPackageReq
set UpdatedID = MUser.Name, CreatedID = MUser.Name
FROM  MUser
where FabPackageReq.UpdatedUser = MUser.account;
update FabPackageReq
set UpdatedID = '', CreatedID = ''
where UpdatedUser is null;
update FabPackageReq
set CreatedDate = UpdatedDate;

update Lot
set CreatedID = MUser.Name
from MUser
where MUser.id = Lot.CreatedUser;
update Lot
set UpdatedID = MUser.Name
from MUser
where MUser.id = Lot.UpdatedUser;

update LotSNAP
set CreatedID = MUser.Name
from MUser
where MUser.id = LotSNAP.CreatedUser;
update LotSNAP
set UpdatedID = MUser.Name
from MUser
where MUser.id = LotSNAP.UpdatedUser;

update PackageMaterial
set CreatedID = MUser.Name
from MUser
where MUser.id = PackageMaterial.CreatedUser;
update PackageMaterial
set UpdatedID = MUser.Name
from MUser
where MUser.id = PackageMaterial.UpdatedUser;

update Production
set UpdatedID = MUser.Name
from MUser
where MUser.id = Production.ModifiedUser;
update Production
set CreatedID = UpdatedID, UpdatedDate = LastModifiedDate;


update RMDetailApplyLog
set CreatedID = CreatedUser;

update RMApplyLog
set CreatedID = CreatedUser;

update SPCSheet
set CreatedID = CreatedUser;

update FabProcessLog
set CreatedID = CreatedUser, ProcessedID=ProcessedUser;

update LotComment
set CreatedID = CreatedUser, UpdatedID = UpdatedUser;

update PMDetailApplyLog
set CreatedID = CreatedUser;

update VitaAidFinishProduct
set CreatedID = CreatedUser, UpdatedID = UpdatedUser, ReleasedID = ReleasedUser;

update VitaAidFinishProductSNAP
set CreatedID = CreatedUser, UpdatedID = UpdatedUser, ReleasedID = ReleasedUser;

update VitaAidSemiProduct
set CreatedID = CreatedUser, UpdatedID = UpdatedUser;

update VitaAidSemiProductSNAP
set CreatedID = CreatedUser, UpdatedID = UpdatedUser;


update ProductPackageReq
set CreatedID = UpdatedUser, CreatedDate = UpdatedDate


---- VAOrder CreatedUser, UpdatedUser => CreatedID, UpdatedID
---- VAOrderItem CreatedUser, UpdatedUser => CreatedID, UpdatedID

/*EXCELL DB
update TestRequest
set CreatedID = MUser.Name
from MUser
where MUser.id = TestRequest.CreatedUser
update TestRequest
set UpdatedID = MUser.Name
from MUser
where MUser.id = TestRequest.UpdatedUser;
*/