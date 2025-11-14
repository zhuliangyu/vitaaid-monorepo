(function() {
  var selectedIds;
  function onMemberInit(s, e) {
    AddAdjustmentDelegate(adjustMember);
    updateToolbarButtonsState();
    DoRefresh();
  }
  function onMemberSelectionChanged(s, e) {
    updateToolbarButtonsState();
  }
  function onMemberRefreshEditFormCallback(s, e) {
    DoRefresh();
  }
  function DoRefresh() {
    memberView.GetRowValues(memberView.GetFocusedRowIndex(), "ID", function(
      values
    ) {
      $.ajax({
        type: "POST",
        url: "Member/MemberEditFormPartial",
        data: { id: values },
        success: function(response) {
          $("#memberEditableContainer").html(response);
        }
      });
    });
  }
  function onMemberEndCallback(s, e) {
    if (e.command == "UPDATEEDIT") {
      if ($("#newMemberForm").length == 0) DoRefresh();
    } else if (e.command == "CUSTOMCALLBACK") {
      DoRefresh();
    }
  }
  function adjustMember() {
    memberView.AdjustControl();
  }
  function updateToolbarButtonsState() {
    var enabled = memberView.GetSelectedRowCount() > 0;
    pageToolbar.GetItemByName("Delete").SetEnabled(enabled);
    pageToolbar
      .GetItemByName("Save")
      .SetEnabled(memberView.GetFocusedRowIndex() !== -1);

    var btnSendActivationEmail = pageToolbar.GetItemByName(
      "SendActivationEmail"
    );
    if (memberView.GetFocusedRowIndex() >= 0) {
      memberView.GetRowValues(
        memberView.GetFocusedRowIndex(),
        "MemberStatus",
        function(values) {
          btnSendActivationEmail.SetEnabled(values == "ACTIVE");
        }
      );
    } else {
      btnSendActivationEmail.SetEnabled(false);
    }
  }
  function onMemberPageToolbarItemClick(s, e) {
    switch (e.item.name) {
      case "ToggleFilterPanel":
        toggleFilterPanel();
        break;
      case "New":
        memberView.AddNewRow();
        break;
      case "Save":
        onDoSaveMember();
        alert("Save successfully!");
        break;
      case "Delete":
        deleteSelectedRecords();
        break;
      case "SendActivationEmail":
        DoSendActivationEmail();
        break;
    }
  }
  function onDoSaveMember() {
    var uploadCtl = MVCxClientUploadControl.Cast(ucDragAndDrop);
    var filesForUploading = uploadCtl.GetSelectedFiles();
    if (filesForUploading != null && filesForUploading.length > 0) {
      uploadCtl.Upload();
    } else {
      DoSaveDataExceptLicencePhoto();
    }
  }

  function onLicencePhotoChanged(s, e) {
    var filesForUploading = s.GetSelectedFiles();
    if (filesForUploading == null || filesForUploading.length == 0) return;
    var txtID = ASPxClientTextBox.Cast(ID);
    var fileNameInServer =
      "S_" + txtID.GetValue() + "_" + filesForUploading[0].name;

    var txtLicencePhoto = ASPxClientTextBox.Cast(LicencePhoto);
    txtLicencePhoto.SetValue(fileNameInServer);

    var txtFileNameInServer = ASPxClientTextBox.Cast(
      FileNameInServer_hiddenField
    );
    txtFileNameInServer.value = fileNameInServer;
    $(".licence-photo-block").css("opacity", "0.1");
  }

  function onUploadControlFileUploadComplete(s, e) {
    DoSaveDataExceptLicencePhoto();
  }
  function DoSaveDataExceptLicencePhoto() {
    $("#memberBasicInfoForm").submit();
    memberView.Refresh();
    DoRefresh();
  }
  function onSendEmail(s, e) {
    var sSalesRep = MVCxClientComboBox.Cast(SalesRep).value;
    if (sSalesRep == "") {
      alert("Please select Sales Representative!");
      return;
    }
    var txtID = ASPxClientTextBox.Cast(ID);
    onDoSaveMember();
    $.ajax({
      type: "POST",
      url: "Member/SendEmailToSalesRep",
      data: { ID: txtID.GetValue() },
      success: function(response) {
        alert(response);
      }
    });
  }
  function onAddToConstantContact(s, e) {
    var txtName = ASPxClientTextBox.Cast(Name).GetValue();
    if (txtName == null || txtName == "") {
      alert("Name can not be empty !");
      return;
    }

    var txtEmail = ASPxClientTextBox.Cast(Email).GetValue();
    if (txtEmail == null || txtEmail == "") {
      alert("Email can not be empty !");
      return;
    }

    var txtID = ASPxClientTextBox.Cast(ID).GetValue();

    var urlstr =
      "SigninConstantContact.asp?id=" +
      txtID +
      "&email=" +
      txtEmail +
      "&name=" +
      txtName;
    $.ajax({
      type: "POST",
      cache: false,
      url: urlstr,
      data: $(this).serializeArray(),
      success: function(data) {
        alert(data);
        return false;
      }
    });
  }
  function DoSendActivationEmail() {
    var txtID = ASPxClientTextBox.Cast(ID);
    $.ajax({
      type: "POST",
      url: "Member/SendActivationEmail",
      data: { ID: txtID.GetValue() },
      success: function(response) {
        alert(response);
      }
    });
  }
  function deleteSelectedRecords() {
    var message = "Confirm delete ";
    var i = 0;
    memberView.GetSelectedFieldValues("Name;Email", function(values) {
      message += values.length + " member(s):";
      for (let i = 0; i < values.length; i++)
        message += "[Name:" + values[i][0] + "|Email: " + values[i][1] + "]";
      message += " ?";
      if (confirm(message)) {
        memberView.GetSelectedFieldValues("ID", getSelectedFieldValuesCallback);
      }
    });
  }
  function toggleFilterPanel() {
    filterPanel.Toggle();
  }

  function onMemberFilterPanelExpanded(s, e) {
    adjustPageControls();
    searchButtonEdit.SetFocus();
  }

  function onMemberBeginCallback(s, e) {
    e.customArgs["SelectedRows"] = selectedIds;
  }
  function getSelectedFieldValuesCallback(values) {
    selectedIds = values.join(",");
    memberView.PerformCallback({ customAction: "delete" });
  }

  window.onMemberBeginCallback = onMemberBeginCallback;
  window.onMemberEndCallback = onMemberEndCallback;
  window.onMemberInit = onMemberInit;
  window.onMemberSelectionChanged = onMemberSelectionChanged;
  window.onMemberPageToolbarItemClick = onMemberPageToolbarItemClick;
  window.onMemberFilterPanelExpanded = onMemberFilterPanelExpanded;
  window.onMemberRefreshEditFormCallback = onMemberRefreshEditFormCallback;
  window.onUploadControlFileUploadComplete = onUploadControlFileUploadComplete;
  window.onLicencePhotoChanged = onLicencePhotoChanged;
  window.onSendEmail = onSendEmail;
  window.onAddToConstantContact = onAddToConstantContact;
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

function saveThumbnailFileName(s, e) {
  document.getElementById("ThumbnailImage_hiddenFieldID").value =
    s.stateObject.uploadedFileName;
}
