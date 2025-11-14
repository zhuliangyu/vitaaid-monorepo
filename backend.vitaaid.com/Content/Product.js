(function() {
  var selectedIds;
  function onProductInit(s, e) {
    AddAdjustmentDelegate(adjustProduct);
    updateToolbarButtonsState();
    DoRefresh();
  }
  function onProductSelectionChanged(s, e) {
    updateToolbarButtonsState();
  }
  function onRefreshEditFormCallback(s, e) {
    DoRefresh();
  }
  function DoRefresh() {
    productView.GetRowValues(productView.GetFocusedRowIndex(), "ID", function(
      values
    ) {
      RefreshEditForm(values);
      RefreshPCImagesMode(values);
      RefreshIngredient(values);
    });
  }
  function RefreshEditForm(ProductID) {
    $.ajax({
      type: "POST",
      url: "Product/ProductEditFormPartial",
      data: { id: ProductID },
      success: function(response) {
        $("#productEditableContainer").html(response);
      }
    });
  }
  function RefreshPCImagesMode(ProductID) {
    $.ajax({
      type: "POST",
      url: "Product/ProductImageDetailPartial",
      data: { ProductID: ProductID },
      success: function(response) {
        //$("#pcImagesModeBody").html(response);
        $("#ImagesListBody").html(response);
      }
    });
  }
  function RefreshIngredient(ProductID) {
    $.ajax({
      type: "POST",
      url: "Product/ProductIngredientPartial",
      data: { ProductID: ProductID },
      success: function(response) {
        //$("#pcImagesModeBody").html(response);
        $("#ingredientBody").html(response);
      }
    });
  }

  function onProductEndCallback(s, e) {
    if (e.command == "UPDATEEDIT") {
      if ($("#newProductForm").length == 0) DoRefresh();
    } else if (e.command == "CUSTOMCALLBACK") {
      DoRefresh();
    }
  }

  function adjustProduct() {
    productView.AdjustControl();
  }
  function updateToolbarButtonsState() {
    var enabled = productView.GetSelectedRowCount() > 0;
    pageToolbar.GetItemByName("Delete").SetEnabled(enabled);
    pageToolbar
      .GetItemByName("Save")
      .SetEnabled(productView.GetFocusedRowIndex() !== -1);
  }
  function onPageToolbarItemClick(s, e) {
    switch (e.item.name) {
      case "ToggleFilterPanel":
        toggleFilterPanel();
        break;
      case "New":
        productView.AddNewRow();
        break;
      case "Save":
        //updateEditForm();
        //productView.StartEditRow(productView.GetFocusedRowIndex());
        //updateEditForm();
        //var editForm = ASPxClientFormLayout.Cast('pvEditForm');
        //var sds = editForm.GetItemByName("ProductCode");//.GetValue();
        //var xcsds = $("#ProductCode_I").val();
        //var sss = editForm.GetByName('ProductCode');
        //var xx = sss.GetValue();

        //var gv = ASPxClientGridView.Cast('productView');
        //var idx = gv.GetFocusedRowIndex();
        //gv.SetEditValue("Size", gv)
        //var id = 0;
        //productView.GetRowValues(productView.GetFocusedRowIndex(), "ID", function (values) {
        //    id = values;
        //    $.ajax({
        //        type: "POST",
        //        url: 'Product/ProductEditFormUpdatePartial',
        //        data: { id: id, ProductCode: 'VA-090' },
        //        success: function (response) {
        //            //$("#productEditableContainer").html(response);
        //        }
        //    });
        //});
        //$("#productBasicInfoForm").submit();
        //alert("Save successfully!");
        //productView.Refresh();
        onDoSaveProduct();
        alert("Save successfully!");
        break;
      case "Delete":
        deleteSelectedRecords();
        break;
      //    case "Export":
      //        productView.ExportTo(ASPxClientProductExportFormat.Xlsx);
      //        break;
    }
  }
  function onDoSaveProduct() {
    var uploadCtl = MVCxClientUploadControl.Cast(ucDragAndDrop);
    var filesForUploading = uploadCtl.GetSelectedFiles();
    if (filesForUploading != null && filesForUploading.length > 0) {
      uploadCtl.Upload();
    } else {
      DoSaveDataExceptProductSheet();
    }
  }

  function onProductSheetChanged(s, e) {
    var filesForUploading = s.GetSelectedFiles();
    if (filesForUploading == null || filesForUploading.length == 0) return;
    var fileNameInServer = filesForUploading[0].name;

    var txtProductSheet = ASPxClientTextBox.Cast(ProductSheet);
    txtProductSheet.SetValue(fileNameInServer);

    var txtFileNameInServer = ASPxClientTextBox.Cast(
      FileNameInServer_hiddenField
    );
    txtFileNameInServer.value = fileNameInServer;
    $(".product-sheet-block").css("opacity", "0.1");
  }

  function onUploadControlFileUploadComplete(s, e) {
    DoSaveDataExceptProductSheet();
  }
  function DoSaveDataExceptProductSheet() {
    $("#productBasicInfoForm").submit();
    productView.Refresh();
    DoRefresh();
  }

  function deleteSelectedRecords() {
    var message = "Confirm delete ";
    var i = 0;
    productView.GetSelectedFieldValues("ProductCode;ProductName", function(
      values
    ) {
      message += values.length + " product(s):";
      for (let i = 0; i < values.length; i++)
        message += "[Code:" + values[i][0] + "|Name. " + values[i][1] + "]";
      message += " ?";
      if (confirm(message)) {
        productView.GetSelectedFieldValues(
          "ID",
          getSelectedFieldValuesCallback
        );
      }
    });
  }
  function onFiltersNavBarItemClick(s, e) {
    var filters = {
      All: "",
      Active: "[Status] = 1",
      Bugs: "[Kind] = 1",
      Suggestions: "[Kind] = 2",
      HighPriority: "[Priority] = 1"
    };
    productView.ApplyFilter(filters[e.item.name]);
    //var pv = ASPxClientGridView.Cast('productView');
    //pv.UpdateEdit();
    HideLeftPanelIfRequired();
  }

  function toggleFilterPanel() {
    filterPanel.Toggle();
  }

  function onFilterPanelExpanded(s, e) {
    adjustPageControls();
    searchButtonEdit.SetFocus();
  }

  function onProductBeginCallback(s, e) {
    e.customArgs["SelectedRows"] = selectedIds;
  }
  function getSelectedFieldValuesCallback(values) {
    selectedIds = values.join(",");
    productView.PerformCallback({ customAction: "delete" });
  }

  function afterProductImageCallback(s, e) {
    if (e.command == "UPDATEEDIT" || e.command == "DELETEROW")
      productView.GetRowValues(productView.GetFocusedRowIndex(), "ID", function(
        values
      ) {
        RefreshPCImagesMode(values);
      });
  }

  window.onProductBeginCallback = onProductBeginCallback;
  window.onProductEndCallback = onProductEndCallback;
  window.onProductInit = onProductInit;
  window.onProductSelectionChanged = onProductSelectionChanged;
  window.onPageToolbarItemClick = onPageToolbarItemClick;
  window.onFilterPanelExpanded = onFilterPanelExpanded;
  window.onFiltersNavBarItemClick = onFiltersNavBarItemClick;
  window.onRefreshEditFormCallback = onRefreshEditFormCallback;
  window.afterProductImageCallback = afterProductImageCallback;
  window.onUploadControlFileUploadComplete = onUploadControlFileUploadComplete;
  window.onProductSheetChanged = onProductSheetChanged;
})();

function getSelectedItemsText(items) {
  var texts = [];
  for (var i = 0; i < items.length; i++) texts.push(items[i].text);
  return texts.join(textSeparator);
}
function getValuesByTexts(chkListBox, texts) {
  var actualValues = [];
  var item;
  for (var i = 0; i < texts.length; i++) {
    item = chkListBox.FindItemByText(texts[i]);
    if (item != null) actualValues.push(item.value);
  }
  return actualValues;
}

function saveFileName(s, e) {
  var target = s.callbackUrl.split("?")[1].split("=")[1];
  document.getElementById(target.concat("_hiddenFieldID")).value =
    s.stateObject.uploadedFileName;
}

function panelResize(s, e) {
  if (e.pane.name == "Pane 1 - 2 - 1")
    productImgView.SetHeight(e.pane.GetClientHeight());
  if (e.pane.name == "Pane 1 - 2 - 2")
    productIngredientView.SetHeight(e.pane.GetClientHeight());
}
