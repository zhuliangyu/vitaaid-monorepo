(function () {
    var selectedIds;
    function onSystemCategoryInit(s, e) {
        AddAdjustmentDelegate(adjustSystemCategory);
        updateToolbarButtonsState();
    }
    function onSystemCategorySelectionChanged(s, e) {
        updateToolbarButtonsState();
    }
    function adjustSystemCategory() {
        systemCategory.AdjustControl();
    }
    function updateToolbarButtonsState() {
        var enabled = systemCategory.GetSelectedRowCount() > 0;
        pageToolbar.GetItemByName("Delete").SetEnabled(enabled);
        //pageToolbar.GetItemByName("Export").SetEnabled(enabled);

        pageToolbar.GetItemByName("Edit").SetEnabled(systemCategory.GetFocusedRowIndex() !== -1);
    }
    function onPageToolbarItemClick(s, e) {
        switch (e.item.name) {
            case "ToggleFilterPanel":
                toggleFilterPanel();
                break;
            case "New":
                systemCategory.AddNewRow();
                break;
            case "Edit":
                systemCategory.StartEditRow(systemCategory.GetFocusedRowIndex());
                break;
            case "Delete":
                deleteSelectedRecords();
                break;
        //    case "Export":
        //        systemCategory.ExportTo(ASPxClientSystemCategoryExportFormat.Xlsx);
        //        break;
        }
    }
    function deleteSelectedRecords() {
        if (confirm('Confirm Delete?')) {
            systemCategory.GetSelectedFieldValues("ID", getSelectedFieldValuesCallback);
        }
    }

    function toggleFilterPanel() {
        filterPanel.Toggle();
    }

    function onFilterPanelExpanded(s, e) {
        adjustPageControls();
        searchButtonEdit.SetFocus();
    }

    function onSystemCategoryBeginCallback(s, e) {
        e.customArgs['SelectedRows'] = selectedIds;
    }
    function getSelectedFieldValuesCallback(values) {
        selectedIds = values.join(',');
        systemCategory.PerformCallback({ customAction: 'delete' });
    }

    window.onSystemCategoryBeginCallback = onSystemCategoryBeginCallback;
    window.onSystemCategoryInit = onSystemCategoryInit;
    window.onSystemCategorySelectionChanged = onSystemCategorySelectionChanged;
    window.onPageToolbarItemClick = onPageToolbarItemClick;
    window.onFilterPanelExpanded = onFilterPanelExpanded;
})();