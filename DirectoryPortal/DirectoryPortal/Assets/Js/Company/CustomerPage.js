var g_CompanyID = jQuery('#g_HiddenCompanyID').val();
var g_AuthorizedToken = jQuery('#g_HiddenAuthorizedToken').val();
var g_ServiceUrl = jQuery('#g_HiddenServiceUrl').val();
var g_LocalUrl = jQuery('#g_HiddenLocalUrl').val();

var g_TempSelectedSortColumn = "";
var g_TempSelectedSortOrder = true;

$("#mainTableFilterSearch").keyup(function (e) {

    if (e.keyCode === 13) {
        GetAllCustomersByPage(1);
    }
});

GetAllCustomersByPage(1);

function GetAllCustomersByPage(pageIndex, selectedSortColumn, selectedSortOrder) {

    if (selectedSortColumn === g_TempSelectedSortColumn) { g_TempSelectedSortOrder = g_TempSelectedSortOrder === true ? false : true; } else { g_TempSelectedSortOrder = true; }

    g_TempSelectedSortColumn = selectedSortColumn === undefined ? "" : selectedSortColumn;

    var pageSize = jQuery('#mainTablePageSize').val();

    var filterStatus = jQuery('#mainTableFilterStatus').val();
    var filterSearch = jQuery('#mainTableFilterSearch').val();

    var params = "?companyID=" + g_CompanyID + "&pageIndex=" + pageIndex + "&pageSize=" + pageSize + "&sortColumn=" + g_TempSelectedSortColumn + "&sortOrder=" + g_TempSelectedSortOrder + "&filterStatus=" + filterStatus + "&filterSearch=" + filterSearch;

    jQuery.support.cors = true;
    $.ajax({
        type: "GET",
        url: g_ServiceUrl + "/api/Customer/GetAllCustomersByPage" + params,
        dataType: 'json',
        cache: false,
        headers: { "Authorization": "Bearer " + g_AuthorizedToken },
        success: function (data) {
            if (data !== null && data !== undefined && data !== "undefined" && data !== "") {

                WriteResponseAllCustomersByPage(data.Result);
                WritePaginationAllCustomersByPage(pageIndex, data.AllItemCount, pageSize);
            } else {

                alertify.error("Giriş işleminiz sırasında bir hata oluştu!");
            }
        },
        error: function (xhr, txtStatus, errorThrown) {
            alertify.error("Hata Kodu:" + xhr.status + " " + txtStatus + "\n" + errorThrown);
        }
    });
}

function WriteResponseAllCustomersByPage(customers) {

    var strResponse = '';
    jQuery.each(customers, function (index, customer) {

        var statusColor = "#f0ad4e", statusTitle = "Passive";
        if (customer.Status === 1) { statusColor = "#5bc0de"; statusTitle = "Active"; }
        if (customer.Status === -1) { statusColor = "#dc2e2e"; statusTitle = "Deleted"; }

        strResponse += '<tr id="customer' + customer.ID + '">';
        strResponse += '<td> <img id="imgCustomer' + customer.ID + '" class="imgTable" src = "/Assets/Image/user-silhouette.png" ></td>';
        strResponse += '<td>' + customer.Name + '</td>';
        strResponse += '<td>' + customer.Email + '</td>';
        strResponse += '<td>' + customer.Phone + '</td>';
        strResponse += '<td>' + (customer.Description !== null ? customer.Description : '') + '</td>';
        strResponse += '<td> <i class="fa fa-circle" title="' + statusTitle + '" style="color:' + statusColor + '"></i></td>';

        strResponse += '<td> <a onclick="GetCustomerForUpdate(' + customer.ID + ');return false;" href="#modalCustomerCreateOrUpdate" data-toggle="modal" title="Update"><i class="fas fa-edit"></i></a></td>';

        if (customer.Status !== -1) {

            strResponse += '<td> <a onclick="jQuery(\'#inputCustomerID\').val(' + customer.ID + ');jQuery(\'#inputCustomerRollback\').val(false); return false;" href="#modalCustomerDeleteOrRollback" data-toggle="modal" title="Delete"><i class="fas fa-trash-alt"></i></a></td>';
        } else {

            strResponse += '<td> <a onclick="jQuery(\'#inputCustomerID\').val(' + customer.ID + ');jQuery(\'#inputCustomerRollback\').val(true); return false;" href="#modalCustomerDeleteOrRollback" data-toggle="modal" title="Rollback"><i class="fas fa-undo"></i></a></td>';
        }
        strResponse += '</tr>';
    });

    jQuery('#mainTableBody').html(strResponse);
}

function WritePaginationAllCustomersByPage(currentPageIndex, allItemCount, currentPageSize) {

    var strResult = "", pageCount = Math.floor(allItemCount / currentPageSize), startIndex = currentPageIndex - 2, finisIndex = currentPageIndex;
    if (allItemCount % currentPageSize !== 0) { pageCount++; }

    if (startIndex < 0) { startIndex = 0; if (finisIndex < pageCount) finisIndex = 2; }
    if (finisIndex < pageCount) { finisIndex++; }

    if (currentPageIndex > 4) { strResult += '<li><a class="input-sm cursorPointer" onclick="GetAllCustomersByPage(1); return false;" aria-label="Previous"><i  class="fa fa-angle-double-left"></i></a></li>'; }
    if (currentPageIndex > 1) { strResult += '<li><a class="input-sm cursorPointer" onclick="GetAllCustomersByPage(' + (currentPageIndex - 1) + ');return false;"><i  class="fa fa-chevron-left"></i></a></li>'; }
    for (var i = startIndex; i < finisIndex; i++) { strResult += '<li><a class="input-sm cursorPointer" onclick="GetAllCustomersByPage(' + (i + 1) + '); return false;">' + (i + 1) + '</a></li>'; }
    if (currentPageIndex < pageCount) { strResult += '<li><a class="input-sm cursorPointer" onclick="GetAllCustomersByPage(' + (currentPageIndex + 1) + ');return false;"><i class="fa fa-chevron-right"></i></a></li>'; }
    if (currentPageIndex < pageCount - 3) { strResult += '<li><a class="input-sm cursorPointer" onclick="GetAllCustomersByPage(' + pageCount + '); return false;" aria-label="Next"><i  class="fa fa-angle-double-right"></i></a></li>'; }

    jQuery("#mainTablePagination").html(strResult);

    var strTotalCountShow = "Customers " + (((currentPageIndex - 1) * currentPageSize) + 1) + "-" + (((currentPageIndex) * currentPageSize)) + " / " + allItemCount + " Total";
    jQuery("#mainTablePaginationInfo").html(strTotalCountShow);

}

function CleanCustomerForUpdate() {

    jQuery('#inputCustomerID').val(0);
    jQuery('#inputCustomerName').val('');
    jQuery('#inputCustomerEmail').val('');
    jQuery('#inputCustomerDescription').val('');
    document.getElementById('inputCustomerStatus').checked = true;
}

function CreateOrUpdateCusotmer() {

    var control = jQuery('#formCustomerCreateOrUpdate').parsley().validate();
    if (control === false)
        return;

    var customerID = jQuery('#inputCustomerID').val();

    var customer = {
        CompanyID: g_CompanyID,
        ID: customerID,
        Name: jQuery('#inputCustomerName').val(),
        Email: jQuery('#inputCustomerEmail').val(),
        Description: jQuery('#inputCustomerDescription').val(),
        Status: jQuery('#inputCustomerStatus').is(':checked') === true ? 1 : 0
    };

    var payload = JSON.stringify(customer);
    jQuery.support.cors = true;
    $.ajax({
        type: customerID === "0" ? "POST" : "PUT",
        url: g_ServiceUrl + "/api/Customer/" + (customerID === "0" ? "CreateCustomer" : "UpdateCustomer"),
        dataType: 'json',
        data: payload,
        contentType: "application/json;charset=utf-8",
        cache: false,
        headers: { "Authorization": "Bearer " + g_AuthorizedToken },
        success: function (data) {

            if (data.HttpStatusCode === 200) {
                GetAllCustomersByPage(1);

                alertify.success("İşleminiz başarıyla tamamlandı!");

                CleanCustomerForUpdate();

                jQuery("#buttonModalCreateOrCustomerClose").trigger("click");
            } else {

                alertify.error("İşleminiz sırasında bir hata oluştu!");
            }
        },
        error: function (xhr, txtStatus, errorThrown) {
            alertify.error("Hata Kodu:" + xhr.status + " " + txtStatus + "\n" + errorThrown);
        }
    });
}

function GetCustomerForUpdate(customerID) {

    var params = "?companyID=" + g_CompanyID + "&customerID=" + customerID;

    jQuery.support.cors = true;
    $.ajax({
        type: "GET",
        url: g_ServiceUrl + "/api/Customer/GetCustomer" + params,
        dataType: 'json',
        cache: false,
        headers: { "Authorization": "Bearer " + g_AuthorizedToken },
        success: function (data) {

            if (data.HttpStatusCode === 200) {

                jQuery('#inputCustomerID').val(data.Result.ID);
                jQuery('#inputCustomerName').val(data.Result.Name);
                jQuery('#inputCustomerEmail').val(data.Result.Email);
                jQuery('#inputCustomerDescription').val(data.Result.Description);
                document.getElementById('inputCustomerStatus').checked = data.Result.Status === 1 ? true : false;

            } else {

                alertify.error("İşleminiz sırasında bir hata oluştu!");
            }
        },
        error: function (xhr, txtStatus, errorThrown) {
            alertify.error("Hata Kodu:" + xhr.status + " " + txtStatus + "\n" + errorThrown);
        }
    });
}

function DeleteOrRolbackCustomer() {

    var customerID = jQuery('#inputCustomerID').val();
    var rollback = jQuery('#inputCustomerRollback').val();

    var params = "?companyID=" + g_CompanyID + "&customerID=" + customerID;

    jQuery.support.cors = true;
    $.ajax({
        type: "DELETE",
        url: g_ServiceUrl + "/api/Customer/" + (rollback === "true" ? "RollbackCustomer" : "DeleteCustomer") + params,
        dataType: 'json',
        cache: false,
        headers: { "Authorization": "Bearer " + g_AuthorizedToken },
        success: function (data) {

            if (data.HttpStatusCode === 200) {
                GetAllCustomersByPage(1);

                alertify.success("İşleminiz başarıyla tamamlandı!");

                jQuery("#buttonModalDeleteOrRollbackClose").trigger("click");
            } else {

                alertify.error("İşleminiz sırasında bir hata oluştu!");
            }
        },
        error: function (xhr, txtStatus, errorThrown) {
            alertify.error("Hata Kodu:" + xhr.status + " " + txtStatus + "\n" + errorThrown);
        }
    });
}