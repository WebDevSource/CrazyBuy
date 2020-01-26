"use strict";

$(document).ready(function () {
  $(".register-modal-link").on("click", function () {
    event.stopPropagation();
    $("#register-modal").modal('show');
    $('#forget-password-modal').modal('hide');
  });
  $('#forget-password-btn').on('click', function () {
    event.stopPropagation();
    $('#forget-password-modal').modal('show');
    $("#register-modal").modal('hide');
  });
  $('#hamburger').on('click', function () {
    if ($("#hamburger").hasClass('active')) {
      $("#hamburger").removeClass('active');
      $(".narbar-logo").removeClass('active');
      $(".navbar-status-content").removeClass('active');
      $(".box").removeClass('active');
      $('.mobile-side-bar').removeClass('active');
    } else {
      $("#hamburger").addClass('active');
      $(".narbar-logo").addClass('active');
      $(".navbar-status-content").addClass('active');
      $(".box").addClass('active');
      $('.mobile-side-bar').addClass('active');
    }
  });
});