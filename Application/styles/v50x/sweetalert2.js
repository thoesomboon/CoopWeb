"use strict"; var KTSweetAlert2Demo = {
    init: function () {

        $("#kt_sweetalert_demo_1").click(function (t) { swal.fire("Good job!") }),
        $("#kt_sweetalert_demo_2").click(function (t) { swal.fire("Here's the title!", "...and here's the text!") }),
        $("#kt_sweetalert_demo_3_1").click(function (t) { swal.fire("Good job!", "You clicked the button!", "warning") }),
        $("#kt_sweetalert_demo_3_2").click(function (t) { swal.fire("Good job!", "You clicked the button!", "error") }),
        $("#kt_sweetalert_demo_3_3").click(function (t) { swal.fire("Good job!", "You clicked the button!", "success") }),
        $("#kt_sweetalert_demo_3_4").click(function (t) { swal.fire("Good job!", "You clicked the button!", "info") }),
        $("#kt_sweetalert_demo_3_5").click(function (t) { swal.fire("Good job!", "You clicked the button!", "question") }),
        $("#kt_sweetalert_demo_4").click(function (t) {
            swal.fire({ title: "Good job!", text: "You clicked the button!", type: "success", confirmButtonText: "Confirm me!", confirmButtonClass: "btn btn-focus kt-btn kt-btn--pill kt-btn--air" })
        }), $("#kt_sweetalert_demo_5").click(function (t) {
            swal.fire({ title: "Good job!", text: "You clicked the button!", type: "success", confirmButtonText: "<span><i class='la la-headphones'></i><span>I am game!</span></span>", confirmButtonClass: "btn btn-danger kt-btn kt-btn--pill kt-btn--air kt-btn--icon", showCancelButton: !0, cancelButtonText: "<span><i class='la la-thumbs-down'></i><span>No, thanks</span></span>", cancelButtonClass: "btn btn-secondary kt-btn kt-btn--pill kt-btn--icon" })
        }), $("#kt_sweetalert_demo_6").click(function (t) {
            swal.fire({ position: "top-right", type: "success", title: "Your work has been saved", showConfirmButton: !1, timer: 1500 })
        }), $("#kt_sweetalert_demo_7").click(function (t) {
            swal.fire({
                title: "jQuery HTML example", html: $("<div>").addClass("some-class").text("jQuery is everywhere."), animation: !1, customClass: "animated tada"
            })
        }), $("#kt_sweetalert_demo_8").click(function (t) {
            swal.fire({
                title: "Are you sure?",
                text: "You won't be able to revert this!",
                type: "warning",
                showCancelButton: !0,
                confirmButtonText: "Yes, delete it!"
            }).then(function (t) { t.value && swal.fire("Deleted!", "Your file has been deleted.", "success") })
        }), $("#kt_sweetalert_demo_9").click(function (t) {
            swal.fire({
                title: "Are you sure?", text: "You won't be able to revert this!",
                type: "warning", showCancelButton: !0,
                confirmButtonText: "Yes, delete it!",
                cancelButtonText: "No, cancel!",
                reverseButtons: !0
            }).then(function (t) {
                t.value ? swal.fire("Deleted!", "Your file has been deleted.", "success") : "cancel" === t.dismiss && swal.fire("Cancelled", "Your imaginary file is safe :)", "error")
            })
        }), $("#kt_sweetalert_demo_10").click(function (t) {
            swal.fire({
                title: "Sweet!", text: "Modal with a custom image.",
                imageUrl: "https://unsplash.it/400/200",
                imageWidth: 400, imageHeight: 200,
                imageAlt: "Custom image",
                animation: !1
            })
        }), $("#kt_sweetalert_demo_11").click(function (t) {
            swal.fire({
                title: "Auto close alert!",
                text: "I will close in 5 seconds.",
                timer: 5e3,
                onOpen: function () {
                    swal.showLoading()
                }
            }).then(function (t) { "timer" === t.dismiss && console.log("I was closed by the timer") })
        })
    }
}; jQuery(document).ready(function () {
    KTSweetAlert2Demo.init()
});