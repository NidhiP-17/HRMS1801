/*** Navigation ***/
jQuery(function ($) {
  $(".sidebar-dropdown > a").click(function() {
    $(".sidebar-submenu").slideUp(200);
    if (
      $(this)
        .parent()
        .hasClass("active")
    ) {
      $(".sidebar-dropdown").removeClass("active");
      $(this)
        .parent()
        .removeClass("active");
    } else {
      $(".sidebar-dropdown").removeClass("active");
      $(this)
        .next(".sidebar-submenu")
        .slideDown(200);
      $(this)
        .parent()
        .addClass("active");
    }
  });


  $("#close-sidebar").click(function() {
    $(".Wrapper").removeClass("toggled");
    $(".nav").toggle();
  });
  $("#show-sidebar").click(function() {
    $(".Wrapper").addClass("toggled");
    $(".nav").toggle();
  });
});




/* When the user clicks on the button, 
toggle between hiding and showing the dropdown content */
function myFunction() {
  document.getElementById("myDropdown").classList.toggle("show");
}

// Close the dropdown if the user clicks outside of it
window.onclick = function(event) {
  if (!event.target.matches('.dropbtn')) {
    var dropdowns = document.getElementsByClassName("dropdown-content");
    var i;
    for (i = 0; i < dropdowns.length; i++) {
      var openDropdown = dropdowns[i];
      if (openDropdown.classList.contains('show')) {
        openDropdown.classList.remove('show');
      }
    }
  }
}



$(".tab_content").hide();
    $(".tab_content:first").show();
$("ul.tabs li").click(function() {
	  $(".tab_content").hide();
      var activeTab = $(this).attr("rel"); 
      $("#"+activeTab).fadeIn();		
      $("ul.tabs li").removeClass("active");
      $(this).addClass("active");
	  $(".tab_drawer_heading").removeClass("d_active");
	  $(".tab_drawer_heading[rel^='"+activeTab+"']").addClass("d_active");
    });
	$(".tab_drawer_heading").click(function() {
      $(".tab_content").hide();
      var d_activeTab = $(this).attr("rel"); 
      $("#"+d_activeTab).fadeIn();
	  $(".tab_drawer_heading").removeClass("d_active");
      $(this).addClass("d_active");
	  $("ul.tabs li").removeClass("active");
	  $("ul.tabs li[rel^='"+d_activeTab+"']").addClass("active");
    });
	$('ul.tabs li').last().addClass("tab_last");
	$('ul.tabs li').last().addClass("tab_last");



  $(document).ready(function() {
    // executes when HTML-Document is loaded and DOM is ready
   
     
      if (location.hash !== '') $('a[href="' + location.hash + '"]').tab('show');
         return $('a[data-toggle="tab"]').on('shown', function(e) {
         return location.hash = $(e.target).attr('href').substr(1);
       });
     
     
   // document ready  
   });
   
   
   
//    jQuery(".btn-mobile-menu").click(function(){
//     jQuery('body').addClass('mobile-menu-open');
// });
//     jQuery(".btn-close-menu").click(function(){
// jQuery('body').removeClass('mobile-menu-open');
// });

$(".selectBox").on("click", function(e) {
  $(this).toggleClass("show");
  var dropdownItem = e.target;
  var container = $(this).find(".selectBox__value");
  container.text(dropdownItem.text);
  $(dropdownItem)
    .addClass("active")
    .siblings()
    .removeClass("active");
});


$( ".filter-inner > a" ).click(function() {
  $( ".ul-filter-dropdown" ).slideToggle( "slow", function() {
    // Animation complete.
  });
});





$(function(){
  
  // Prevent two submenus from being opened at once
  $('li.dropdown > a').on('click',function(event){
    
$(this).parent().find('ul').first().toggle(300);  
$(this).parent('li').addClass('opened');  
$(this).parent().siblings().find('ul').hide(200);
$(this).parent().siblings('li').removeClass('opened');
$(this).siblings().find('li').removeClass('opened');
     event.preventDefault()
  });
});







$('.sub-menu ul').hide();
$(".sub-menu a").click(function () {
	$(this).parent(".sub-menu").children("ul").slideToggle("100");
	$(this).find(".right").toggleClass("fa-caret-up fa-caret-down");
});


$('.toogles-cust').click(function(){
  $('.Customer').toggle()
});

$('#lg-slider-toggle').click(function(){
  $('.lg-Customer').toggle()
});

$('#lg-slider-toggle1').click(function(){
  $('.lg-Customer1').toggle()
});


$('.btn').click(function(){
  $(this).toggleClass("click");
  $('.sidebar').toggleClass("show");
});
 

  $('.sidebar ul li a').click(function(){
       var id = $(this).attr('id');
       $('nav ul li ul.item-show-'+id).toggleClass("show");
       $('nav ul li #'+id+' span').toggleClass("rotate");
       
  });
  
  $('nav ul li').click(function(){
    $(this).addClass("active").siblings().removeClass("active");
  });








// ====profile-head============

$( "li.profile-head > a" ).click(function() {
  $( ".ul-profile-dropdown" ).slideToggle( "slow", function() {
    // Animation complete.
  });
});


$( "#language-select > a" ).click(function() {
  $( "#language-options" ).slideToggle( "slow", function() {
    // Animation complete.
  });
});

// ===========multipul-languages=================

$(function() {
  
  setCheckboxSelectLabels();
  
  $('.toggle-next').click(function() {
    $(this).next('.checkboxes').slideToggle(400);
  });
  
  $('.ckkBox').change(function() {
    toggleCheckedAll(this);
    setCheckboxSelectLabels(); 
  });
  
});
  
function setCheckboxSelectLabels(elem) {
  var wrappers = $('.wrappers'); 
  $.each( wrappers, function( key, wrapper ) {
    var checkboxes = $(wrapper).find('.ckkBox');
    var label = $(wrapper).find('.checkboxes').attr('id');
    var prevText = '';
    $.each( checkboxes, function( i, checkbox ) {
      var button = $(wrapper).find('button');
      if( $(checkbox).prop('checked') == true) {
        var text = $(checkbox).next().html();
        var btnText = prevText + text;
        var numberOfChecked = $(wrapper).find('input.val:checkbox:checked').length;
        if(numberOfChecked >= 4) {
           btnText = numberOfChecked +' '+ label + ' selected';
        }
        $(button).text(btnText); 
        prevText = btnText + ', ';
      }
    });
  });
}

function toggleCheckedAll(checkbox) {
  var apply = $(checkbox).closest('.wrappers').find('');
  apply.fadeIn('slow'); 
  
  var val = $(checkbox).closest('.checkboxes').find('.val');
  var all = $(checkbox).closest('.checkboxes').find('.all');
  var ckkBox = $(checkbox).closest('.checkboxes').find('.ckkBox');

  if(!$(ckkBox).is(':checked')) {
    $(all).prop('checked', true);
    return;
  }

  if( $(checkbox).hasClass('all') ) {
    $(val).prop('checked', false);
  } else {
    $(all).prop('checked', false);
  }
}



$(document).ready(function() {  
  $('#multiple-checkboxes').multiselect();  
});  





// $('.Catalogues-tab').click(function(){
//   $('.color-gray').css("background-color","red");
  
// });

// $('.color-gray1').css("background-color","red");


$('.Catalogues-flex li:first-child').addClass('active');
$('. Catalogues-tab').hide();
$('. Catalogues-tab:first').show();

// Click function
$('.Catalogues-flex li').click(function(){
  $('.Catalogues-flex li').removeClass('active');
  $(this).addClass('active');
  $('. Catalogues-tab').hide();
  
  var activeTab = $(this).find('a').attr('href');
  $(activeTab).fadeIn();
  return false;
});



$('#tabs-nav li:first-child').addClass('active');
$('.tab-content').hide();
$('.tab-content:first').show();

// Click function
$('#tabs-nav li').click(function(){
  $('#tabs-nav li').removeClass('active');
  $(this).addClass('active');
  $('.tab-content').hide();
  
  var activeTab = $(this).find('a').attr('href');
  $(activeTab).fadeIn();
  return false;
});