(function () {
    var selectedIds;
    function onWebinarInit(s, e) {
        AddAdjustmentDelegate(adjustWebinar);
        updateToolbarButtonsState();
        DoRefresh();
    }
    function onWebinarSelectionChanged(s, e) {
        updateToolbarButtonsState();
    }
    function onWebinarRefreshEditFormCallback(s, e) {
        DoRefresh();
    }
    function DoRefresh() {
        webinarView.GetRowValues(webinarView.GetFocusedRowIndex(), "ID", function (values) {
            $.ajax({
                type: "POST",
                url: 'Webinar/WebinarEditFormPartial',
                data: { id: values },
                success: function (response) {
                    $("#webinarEditableContainer").html(response);
                }
            });

        });
    }
    function onWebinarEndCallback(s, e) {
        if (e.command == 'UPDATEEDIT') {
            if ($("#newWebinarForm").length == 0)
                DoRefresh();
        }
        else if (e.command == 'CUSTOMCALLBACK') {
            DoRefresh();
        }
    }
    function adjustWebinar() {
        webinarView.AdjustControl();
    }
    function updateToolbarButtonsState() {
        var enabled = webinarView.GetSelectedRowCount() > 0;
        pageToolbar.GetItemByName("Delete").SetEnabled(enabled);
        pageToolbar.GetItemByName("Save").SetEnabled(webinarView.GetFocusedRowIndex() !== -1);
    }
    function onWebinarPageToolbarItemClick(s, e) {
        switch (e.item.name) {
            case "ToggleFilterPanel":
                toggleFilterPanel();
                break;
            case "New":
                webinarView.AddNewRow();
                break;
            case "Save":
                $("#webinarBasicInfoForm").submit();
                alert("Save successfully!");
                webinarView.Refresh();
                break;
            case "Delete":
                deleteSelectedRecords();
                break;
        }
    }
    function deleteSelectedRecords() {
        var message = 'Confirm delete ';
        var i = 0;
        webinarView.GetSelectedFieldValues("Issue;Topic", function (values) {
            message += values.length + ' webinar(s):';
            for (let i = 0; i < values.length; i++)
                message += '[Issue:' + values[i][0] + '|topic: ' + values[i][1] + ']';
            message += ' ?';
            if (confirm(message)) {
                webinarView.GetSelectedFieldValues("ID", getSelectedFieldValuesCallback);
            }
        });

        
    }
    function toggleFilterPanel() {
        filterPanel.Toggle();
    }

    function onWebinarFilterPanelExpanded(s, e) {
        adjustPageControls();
        searchButtonEdit.SetFocus();
    }

    function onWebinarBeginCallback(s, e) {
        e.customArgs['SelectedRows'] = selectedIds;
    }
    function getSelectedFieldValuesCallback(values) {
        selectedIds = values.join(',');
        webinarView.PerformCallback({ customAction: 'delete' });
    }

    window.onWebinarBeginCallback = onWebinarBeginCallback;
    window.onWebinarEndCallback = onWebinarEndCallback;
    window.onWebinarInit = onWebinarInit;
    window.onWebinarSelectionChanged = onWebinarSelectionChanged;
    window.onWebinarPageToolbarItemClick = onWebinarPageToolbarItemClick;
    window.onWebinarFilterPanelExpanded = onWebinarFilterPanelExpanded;
    window.onWebinarRefreshEditFormCallback = onWebinarRefreshEditFormCallback;
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

function saveThumbnailFileName(s, e) {    
    document.getElementById('ThumbnailImage_hiddenFieldID').value = s.stateObject.uploadedFileName;
}
