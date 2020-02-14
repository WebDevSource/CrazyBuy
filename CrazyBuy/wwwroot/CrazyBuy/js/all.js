"use strict";

$(document).ready(function () {
  Popper.Defaults.modifiers.computeStyle.gpuAcceleration = false;
  $(window).scroll(function () {
    if ($(this).scrollTop() > 300) {
      $('.gotop').addClass('active');
    } else {
      $('.gotop').removeClass('active');
    }
  });
  $(".gotop").on('click', function () {
    $("html,body").animate({
      scrollTop: 0
    }, 700);
    return false;
  });
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
  $(".navbar-search-btn").on("click", function () {
    event.preventDefault();

    if ($(".navbar-search-text").hasClass('active')) {// do something 
    } else {
      $(".navbar-search-text").addClass('active');
    }
  });
  $('#proudctCarousel').on('slide.bs.carousel', function (e) {
    var id = parseInt($(e.relatedTarget).attr('data-slide-number'));
    $('[id^=carousel-selector-]').removeClass('active');
    $('[id=carousel-selector-' + id + ']').addClass('active');
  });
  $('#multiple-filters').multiselect({
    nonSelectedText: '請選擇(多選)',
    nSelectedText: '個條件',
    numberDisplayed: 1
  });
  $('#filters-allclear').on('click', function () {
    $("#multiple-filters").multiselect('clearSelection');
    $('#filters-form')[0].reset();
  });
  $('.multiselect').on('click', function (e) {
    e.stopPropagation();
    $('.multiselect-container.dropdown-menu').toggleClass('show');
    close_Listener();
  });

  function close_Listener() {
    $('.filters-col').on('click', function (e) {
      $('.multiselect-container.dropdown-menu').removeClass('show');
      remove_Listener();
    });
  }

  function remove_Listener() {
    $('.filters-col').off('click');
  }

  $('.desktop-filters').on('show.bs.dropdown', function () {
    $('.multiselect-container.dropdown-menu').removeClass('show');
  });
  $(document).on('click', '#mobile-nav-product-category .dropdown-menu', function (e) {
    e.stopPropagation();
  });
});