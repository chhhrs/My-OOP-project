function Locking() {
    $.ajax({
        type: 'POST',
        url: "/Account/LockUser",
        data: { arr: getCheckedCheckBoxes() },
        success: function (data) {
            if (data === "True") {
                location.reload();
            } else {

            }
        },
        error: function () {
            alert("Error");
        }
    });
}

function Unlocking() {
    $.ajax(
        {
            type: 'POST',
            url: "/Account/UnLockUser",
            data: { arr: getCheckedCheckBoxes() },
            success: function (data) {
                if (data === "True") {
                    location.reload();
                } else {

                }
            },
            error: function () {
                alert("Error");
            }
        }
    );
}

function Delete() {
    $.ajax(
        {
            type: 'POST',
            url: "/Account/DeleteUser",
            data: { arr: getCheckedCheckBoxes() },
            success: function (data) {
                if (data === "True") {
                    location.reload();
                } else {

                }
            },
            error: function () {
                alert("Error");
            }
        }
    );
}

function getCheckedCheckBoxes() {
    var checkboxes = document.getElementsByClassName('checkbox');
    var checkboxesChecked = [];
    for (var index = 0; index < checkboxes.length; index++) {
        if (checkboxes[index].checked) {
            checkboxesChecked.push(checkboxes[index].value);
        }
    }
    return checkboxesChecked;
}

$(document).ready(function () {
    $("#checkall").change(function () {
        var checked = $(this).is(':checked');
        if (checked) {
            $(".checkbox").each(function () {
                $(this).prop("checked", true);
            });
        } else {
            $(".checkbox").each(function () {
                $(this).prop("checked", false);
            });
        }
    });

    $(".checkbox").click(function () {

        if ($(".checkbox").length == $(".checkbox:checked").length) {
            $("#checkall").prop("checked", true);
        } else {
            $("#checkall").prop("checked", false);
        }

    });
});