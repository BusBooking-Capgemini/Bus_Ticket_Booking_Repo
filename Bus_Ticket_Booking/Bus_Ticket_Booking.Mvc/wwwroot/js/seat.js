let selectedSeat = null;

$(document).ready(function () {

    $(".seat-btn").click(function () {

        // REMOVE OLD SELECTED STYLE

        $(".seat-btn.btn-primary")
            .removeClass("btn-primary")
            .addClass("btn-success");


        // SET NEW SELECTED STYLE

        $(this)
            .removeClass("btn-success")
            .addClass("btn-primary");


        // SAVE SEAT NUMBER

        selectedSeat =
            $(this).data("seat");
    });


    $("#bookSeatBtn").click(function () {

        if (selectedSeat == null) {

            alert(
                "Please select a seat.");

            return;
        }

        const tripId =
            $("#tripId").val();


        // REDIRECT TO BOOKING PAGE

        window.location.href =
            `/Booking/Create?tripId=${tripId}&seatNumber=${selectedSeat}`;
    });

});