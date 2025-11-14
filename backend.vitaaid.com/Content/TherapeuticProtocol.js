(function() {
  var selectedIds;
  function onTherapeuticProtocolInit(s, e) {
    AddAdjustmentDelegate(adjustTherapeuticProtocol);
    updateToolbarButtonsState();
    DoRefresh();
  }
  function onTherapeuticProtocolSelectionChanged(s, e) {
    updateToolbarButtonsState();
  }
  function onTherapeuticProtocolRefreshEditFormCallback(s, e) {
    DoRefresh();
  }
  function DoRefresh() {
    therapeuticProtocolView.GetRowValues(
      therapeuticProtocolView.GetFocusedRowIndex(),
      "ID",
      function(values) {
        $.ajax({
          type: "POST",
          url: "TherapeuticProtocol/TherapeuticProtocolEditFormPartial",
          data: { id: values },
          success: function(response) {
            $("#therapeuticProtocolEditableContainer").html(response);
          }
        });
      }
    );
  }
  function onTherapeuticProtocolEndCallback(s, e) {
    if (e.command == "UPDATEEDIT") {
      if ($("#newTherapeuticProtocolForm").length == 0) DoRefresh();
    } else if (e.command == "CUSTOMCALLBACK") {
      DoRefresh();
    }
  }
  function adjustTherapeuticProtocol() {
    therapeuticProtocolView.AdjustControl();
  }
  function updateToolbarButtonsState() {
    var enabled = therapeuticProtocolView.GetSelectedRowCount() > 0;
    pageToolbar.GetItemByName("Delete").SetEnabled(enabled);
    pageToolbar
      .GetItemByName("Save")
      .SetEnabled(therapeuticProtocolView.GetFocusedRowIndex() !== -1);
  }
  function onTherapeuticProtocolPageToolbarItemClick(s, e) {
    switch (e.item.name) {
      case "ToggleFilterPanel":
        toggleFilterPanel();
        break;
      case "New":
        therapeuticProtocolView.AddNewRow();
        break;
      case "Save":
        onDoSaveProtocol();
        alert("Save successfully!");
        break;
      case "Delete":
        deleteSelectedRecords();
        break;
    }
  }

  function onDoSaveProtocol() {
    var uploadCtl = MVCxClientUploadControl.Cast(ucDragAndDrop);
    var filesForUploading = uploadCtl.GetSelectedFiles();
    if (filesForUploading != null && filesForUploading.length > 0) {
      uploadCtl.Upload();
    } else {
      DoSaveDataExceptPDFFile();
    }
  }

  function deleteSelectedRecords() {
    var message = "Confirm delete ";
    var i = 0;
    therapeuticProtocolView.GetSelectedFieldValues("Issue;Topic", function(
      values
    ) {
      message += values.length + " TherapeuticProtocol(s):";
      for (let i = 0; i < values.length; i++)
        message += "[Issue:" + values[i][0] + "|topic: " + values[i][1] + "]";
      message += " ?";
      if (confirm(message)) {
        therapeuticProtocolView.GetSelectedFieldValues(
          "ID",
          getSelectedFieldValuesCallback
        );
      }
    });
  }

  function onPDFChanged(s, e) {
    var filesForUploading = s.GetSelectedFiles();
    if (filesForUploading == null || filesForUploading.length == 0) return;
    var fileNameInServer = filesForUploading[0].name;

    var txtPDFFile = ASPxClientTextBox.Cast(PDFFile);
    txtPDFFile.SetValue(fileNameInServer);

    var txtFileNameInServer = ASPxClientTextBox.Cast(
      FileNameInServer_hiddenField
    );
    txtFileNameInServer.value = fileNameInServer;
    $(".pdf-block").css("opacity", "0.1");
  }

  function onUploadControlFileUploadComplete(s, e) {
    DoSaveDataExceptPDFFile();
  }
  function DoSaveDataExceptPDFFile() {
    $("#therapeuticProtocolBasicInfoForm").submit();
    therapeuticProtocolView.Refresh();
    DoRefresh();
  }

  function toggleFilterPanel() {
    filterPanel.Toggle();
  }

  function onTherapeuticProtocolFilterPanelExpanded(s, e) {
    adjustPageControls();
    searchButtonEdit.SetFocus();
  }

  function onTherapeuticProtocolBeginCallback(s, e) {
    e.customArgs["SelectedRows"] = selectedIds;
  }
  function getSelectedFieldValuesCallback(values) {
    selectedIds = values.join(",");
    therapeuticProtocolView.PerformCallback({ customAction: "delete" });
  }

  window.onTherapeuticProtocolBeginCallback = onTherapeuticProtocolBeginCallback;
  window.onTherapeuticProtocolEndCallback = onTherapeuticProtocolEndCallback;
  window.onTherapeuticProtocolInit = onTherapeuticProtocolInit;
  window.onTherapeuticProtocolSelectionChanged = onTherapeuticProtocolSelectionChanged;
  window.onTherapeuticProtocolPageToolbarItemClick = onTherapeuticProtocolPageToolbarItemClick;
  window.onTherapeuticProtocolFilterPanelExpanded = onTherapeuticProtocolFilterPanelExpanded;
  window.onTherapeuticProtocolRefreshEditFormCallback = onTherapeuticProtocolRefreshEditFormCallback;
  window.onUploadControlFileUploadComplete = onUploadControlFileUploadComplete;
  window.onPDFChanged = onPDFChanged;
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

function saveBannerFileName(s, e) {
  document.getElementById("BannerImage_hiddenFieldID").value =
    s.stateObject.uploadedFileName;
}
