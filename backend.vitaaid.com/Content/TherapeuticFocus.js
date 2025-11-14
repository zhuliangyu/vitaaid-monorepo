(function () {
    var selectedIds;
    function onTherapeuticFocusInit(s, e) {
        AddAdjustmentDelegate(adjustTherapeuticFocus);
        updateToolbarButtonsState();
        DoRefresh();
    }
    function onTherapeuticFocusSelectionChanged(s, e) {
        updateToolbarButtonsState();
    }
    function onTFRefreshEditFormCallback(s, e) {
        DoRefresh();
    }
    function DoRefresh() {
        tfView.GetRowValues(tfView.GetFocusedRowIndex(), "ID", function (values) {
            $.ajax({
                type: "POST",
                url: 'TherapeuticFocus/TherapeuticFocusEditFormPartial',
                data: { id: values },
                success: function (response) {
                    $("#therapeuticEditableContainer").html(response);
                }
            });
        });
    }
    function onTherapeuticFocusEndCallback(s, e) {
        if (e.command == 'UPDATEEDIT') {
            if ($("#newTherapeuticFocusForm").length == 0)
                DoRefresh();
        }
        else if (e.command == 'CUSTOMCALLBACK') {
            DoRefresh();
        }
    }
    function adjustTherapeuticFocus() {
        tfView.AdjustControl();
    }
    function updateToolbarButtonsState() {
        var enabled = tfView.GetSelectedRowCount() > 0;
        pageToolbar.GetItemByName("Delete").SetEnabled(enabled);
        pageToolbar.GetItemByName("Save").SetEnabled(tfView.GetFocusedRowIndex() !== -1);
    }
    function onTFPageToolbarItemClick(s, e) {
        switch (e.item.name) {
            case "ToggleFilterPanel":
                toggleFilterPanel();
                break;
            case "New":
                tfView.AddNewRow();
                break;
            case "Save":
                $("#therapeuticBasicInfoForm").submit();
                alert("Save successfully!");
                tfView.Refresh();
                break;
            case "Delete":
                deleteSelectedRecords();
                break;
        }
    }
    function deleteSelectedRecords() {
        var message = 'Confirm delete ';
        var i = 0;
        tfView.GetSelectedFieldValues("Issue;Volume;No", function (values) {
            message += values.length + ' blog(s):';
            for (let i = 0; i < values.length; i++)
                message += '[Issue:' + values[i][0] + '|Vol. ' + values[i][1] + '|No. ' + values[i][2] + ']';
            message += ' ?';
            if (confirm(message)) {
                tfView.GetSelectedFieldValues("ID", getSelectedFieldValuesCallback);
            }
        });

        
    }
    function toggleFilterPanel() {
        filterPanel.Toggle();
    }

    function onTFFilterPanelExpanded(s, e) {
        adjustPageControls();
        searchButtonEdit.SetFocus();
    }

    function onTherapeuticFocusBeginCallback(s, e) {
        e.customArgs['SelectedRows'] = selectedIds;
    }
    function getSelectedFieldValuesCallback(values) {
        selectedIds = values.join(',');
        tfView.PerformCallback({ customAction: 'delete' });
    }

    window.onTherapeuticFocusBeginCallback = onTherapeuticFocusBeginCallback;
    window.onTherapeuticFocusEndCallback = onTherapeuticFocusEndCallback;
    window.onTherapeuticFocusInit = onTherapeuticFocusInit;
    window.onTherapeuticFocusSelectionChanged = onTherapeuticFocusSelectionChanged;
    window.onTFPageToolbarItemClick = onTFPageToolbarItemClick;
    window.onTFFilterPanelExpanded = onTFFilterPanelExpanded;
    window.onTFRefreshEditFormCallback = onTFRefreshEditFormCallback;
})();

function getSelectedItemsText(items) {
    var texts = [];
    for (var i = 0; i < items.length; i++)
        texts.push(items[i].text);
    return texts.join(textSeparator);
}
function getValuesByTexts(chkListBox, texts) {
    var actualValues = [];
    var item;
    for (var i = 0; i < texts.length; i++) {
        item = chkListBox.FindItemByText(texts[i]);
        if (item != null)
            actualValues.push(item.value);
    }
    return actualValues;
}

function saveThumbFileName(s, e) {    
    document.getElementById('ThumbImage_hiddenFieldID').value = s.stateObject.uploadedFileName;
}
function saveBannerFileName(s, e) {
    document.getElementById('BannerImage_hiddenFieldID').value = s.stateObject.uploadedFileName;
}
