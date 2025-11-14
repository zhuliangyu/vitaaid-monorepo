
alter table EDIHLItem 			alter column RetailLotNo nvarchar(40);
alter table VAGiftItem 			alter column RetailLotNo nvarchar(40);
alter table VAOrderItem 		alter column RetailLotNo nvarchar(40);
alter table VAOrderItemByLot 		alter column RetailLotNo nvarchar(40);
alter table VitaAidFinishProduct 	alter column RetailLotNo nvarchar(40);
alter table VitaAidFinishProduct 	alter column FabLotNo nvarchar(40);
alter table VitaAidFinishProductLog 	alter column RetailLotNo nvarchar(40);
alter table VitaAidFinishProductLog 	alter column FabLotNo nvarchar(40);
alter table VitaAidFinishProductSNAP 	alter column RetailLotNo nvarchar(40);
alter table VitaAidFinishProductSNAP 	alter column FabLotNo nvarchar(40);
alter table VitaAidSemiProduct 		alter column RetailLotNo nvarchar(40);
alter table VitaAidSemiProduct 		alter column FabLotNo nvarchar(40);
alter table VitaAidSemiProductSNAP 	alter column RetailLotNo nvarchar(40);
alter table VitaAidSemiProductSNAP 	alter column FabLotNo nvarchar(40);
