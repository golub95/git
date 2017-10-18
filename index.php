<!DOCTYPE html>
<html lang="en">
  <head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="description" content="">
    <meta name="author" content="">
    <link rel="icon" href="../../favicon.ico">
	<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.1.1/jquery.min.js"></script>
	<script src="js/jquery.magnific-popup.js"></script>

    <title>Stranica za MMT</title>
	
    <link href="css/bootstrap.min.css" rel="stylesheet">
    <link href="css/justified-nav.css" rel="stylesheet">
	<link href="css/magnific-popup.css" rel="stylesheet">
	<link href="css/custom.css" rel="stylesheet">

  </head>
	<style>
	img.slika{
		margin-left:120%;
		position: absolute;
		height: 250px;
		width: 250px;
		}
	img.slika1{
		margin-left:120%;
		position: absolute;
		height: 250px;
		width: 250px;
		}
	audio{
		margin-top:45px;
		margin-bottom:40px;
		
	}
	.hover11 figure img {
	opacity: 1;
	-webkit-transition: .3s ease-in-out;
	transition: .3s ease-in-out;
	}
	.hover11 figure:hover img {
	opacity: .5;
	}
	
	</style>

  <body>

    <div class="container">

      <!-- The justified navigation menu is meant for single line per list item.
           Multiple lines will require custom code not provided by Bootstrap. -->
      <div class="masthead">

        <nav>
          <ul class="nav nav-justified">
            <li class="active"><a href="#">Početna</a></li>
			<li><a href="Sadržaj.html">Nastavni sadržaj</a></li>
            <li><a target="_blank" href="https://login.aaiedu.hr/sso/module.php/core/loginuserpass.php?AuthState=_c736d16406dd7821613a8b29b0622181755a9b436e%3Ahttps%3A%2F%2Flogin.aaiedu.hr%2Fsso%2Fsaml2%2Fidp%2FSSOService.php%3Fspentityid%3Dhttps%253A%252F%252Floomen.carnet.hr%252F%26cookieTime%3D1490712696%26RelayState%3Dhttps%253A%252F%252Floomen.carnet.hr%252Fauth%252Fsaml%252Findex.php%253Fwantsurl%253Dhttps%25253A%25252F%25252Floomen.carnet.hr%25252Fmy%25252F">Loomen</a></li>
            <li><a href="#aj">informacije</a></li>
          </ul>
        </nav>
      </div>

      <!-- Jumbotron -->
      <div class="jumbotron">
	  <a href='logo.html'>
	  <div class="logo" width="5px" height="5"><img src="img/logo.png"alt="logo" ></a></div>
        <h1>Osnove elektrotehnike</h1>
        <p class="lead">ECTS 7 | P 45 | A 30 | LV 15 | K 0 | ISVU 74054</p>
		
        <p><a class="btn btn-lg btn-success" href="http://osnove.tel.fer.hr/VJEZBEOE/vjezbe.asp" <a class="text" onclick="AjaxTest();this.style.visibility= 'hidden';" role="button" data-clipboard-text="Kako položiti" data-toggle="tooltip" data-placement="bottom">Interatkivni sadržaj</a></p>
      
	  </div>
	  
      <div class="row">
	  
	     <div class="col-sm-8 blog-main">

          <div class="blog-post">
            <h2 class="blog-post-title">Načini polaganje ispita</h2>
			
			<div class="hover11 column">
			<figure><img class="slika" src="img/OhmovZakon.gif"alt="logo"></figure>
			</div>
            

            <p>Nastava iz kolegija Osnova elektrotehnike koncipirana je na način da studenti kroz semestar imaju mogućnost da kolokvijalno polažu gradivo tijekom semestra. Smisao ovog načina ocjenjivanja je da se studenti stimuliraju na kontinuirani rad tijekom semestra u odnosu na učenje i polaganje ispita klasičnim načinom: pismeni i usmeni ispit. Studentske obaveze su podijeljene na način da su teži dijelovi gradiva rastavljeni na niz jednostavnijih zadataka, jer se iz veće skupina ocjena može se bolje pratiti rad studenata.</p>
            <hr>
            <p>Studenti su obavezni prisustvovati nastavi, te je to nužan uvjet za dobivanje potpisa bez kojega student nema pravo pristupiti ispitu. </p>

              <p>Laboratorijske vježbe su <strong>obavezan dio nastave</strong> te je njihovo kolokviranje drugi preduvjet izlaska na ispit.</p>
            
            <p><strong>Kolokviji auditornih vježbi:</strong> studenti tijekom semestra polažu tri kolokvija (1. elektrostatika i magnetostatika, 2. strujni krugovi istosmjerne struje, 3. strujni krugovi izmjenične struje)te ako prikupe minimalno 40% bodova uz kolokvirane laboratorijske vježbe stječu pravo da pristupe usmenom ispitu bez polaganja pismenog ispita (vrijedi  godinu dana od završetka predavanja).</p>
            
			<div class="hover11 column">
			<figure><img class="slika1" src="img/graf.jpg"alt="logo"></figure>
			</div>

			<p> <strong>Pismeni ispit:</strong> studenti koji nisu uspjeli kolokvirati auditorne vježbe moraju prije izlaska na usmeni ispit položiti pismeni ispit, koji se sastoji od 5 zadataka koji pokrivaju kompletno gradivo. Ispit traje 2 sata i na njemu studenti nemaju pravo koristiti nikakvu literaturu. Položen pismeni ispit vrijedi samo za jedan izlazak na usmeni ispit.</p>
			<p><strong>Usmeni ispit:</strong> studenti tijekom semestra polažu više kolokvija iz teorije kojima stječu pravo na početnu ocjenu usmenog ispita. Ovisno o uspjehu studenta na prethodnim aktivnostima ispitivač na usmenom ispitu gradira broj i težinu pitanja. Ukoliko student ne zadovolji na usmenom ispitu, bodovi koje je prikupio tijekom semestra iz kolokvija mu vrijede za slijedeći usmeni ispit (vrijedi  godinu dana od završetka predavanja)</p>
			<p> <strong>Usmeni ispit:</strong> studenti tijekom semestra polažu više <strong>kolokvija</strong> iz teorije kojima stječu pravo na početnu ocjenu usmenog ispita. Ovisno o uspjehu studenta na prethodnim aktivnostima ispitivač na usmenom ispitu gradira broj i težinu pitanja. Ukoliko student ne zadovolji na usmenom ispitu, bodovi koje je prikupio tijekom semestra iz kolokvija mu vrijede za slijedeći usmeni ispit (vrijedi  godinu dana od završetka predavanja)</p>
			<p> <strong>Konačna ocjena</strong> kojom student polaže kolegij formira se kao srednja vrijednost ocjene pismenog ispita (ili kolokvija auditornih vježbi) i ocjene usmenog ispita (dodatno se priznavaju bodovi kolokvija teorije).</p>
			
			
			
			</div>
			
			 </div>
        </div><!-- /.blog-sidebar -->

      <!-- /.row -->
	<hr>

			  

      <!-- Example row of columns -->
      <div class="row">
        <div class="col-lg-4">
		<center>
          <h2>Najkorisnije što sam naučio na ovom kolegiju</h2>
          <p> <strong>Luka:</strong> "Poznavanje temeljnih zakona elektromagnetizma, veličina i jedinica koje opisuju električno i magnetsko polje; proračuni i mjerenja u jednostavnom strujnom krugu istosmjerne i izmjenične struje. Današnji je život teško zamisliti bez primjene elektrotehnike. Ona ulazi u sve dijelove naše svakodnevnice: od rasvjete, radija i televizije, preko industrijskih i transportnih sustava, do komunikacija i računala."</p>
          <p class="text-danger"><strong>Josip: </strong>"Na fakultetu su nam najvažnije bile Laboratorijske vježbe, gdje smo pokusima utvrđivali stečeno znanje. U zaglavlju klikom na "Interaktivni sadržaj" otvorit će se edukacijski prozor koji prikazuje dio nastavnih sadržaja predmeta Osnove elektrotehnike koji se odnosi se na simulacije i animacije pokusa. Virtualni pokusi odnosno simulacije ne mogu u potpunosti zamijeniti stvarne pokuse, ali mogu pomoći u pripremi njihova izvođenja."</p>

        </div>
        <div class="col-lg-4">
		<center>
          <h3>Instrukcije na daljinu</h3>
		  <video width="370" controls>
			<source src="Video.mp4" type="video/MP4">
			<source src="Video.ogg" type="video/ogg">
			Your browser does not support HTML5 video.
		</video>
		</center>
		<center><a class="btn btn-primary" target="blank"href="https://www.youtube.com/channel/UCkwpw88Pj8agokoXHE_kuEg/videos?flow=grid&view=0&sort=da&live_view=500" role="button">Više &raquo;</a></center>
		
		<center>
		<audio controls>
		<source src="Audio.ogg" type="audio/ogg">
		<source src="Audio.wav" type="audio/mpeg">
	
		</audio>
		</center>
		</div>
      
	   
		
        <div class="col-lg-4">
		<p>
		<a class="simple-ajax-popup-align-top" href='josip.html'>
		<img class="featurette-image img-responsive center-block img-rounded" border="0" alt="JOSIP" src="img/Josip_mala.jpg" width="150" height="150">
		</a>
		<div class="josip">
		<center>
		<h3>
		<a class="josip" onclick="Ajaxjosip();this.style.visibility= 'hidden';" role="button" data-clipboard-text="Kako položiti" data-toggle="tooltip" data-placement="bottom">JOSIP</a></h3>
		</center>
		</p>
		</div>
	<script>
	function Ajaxjosip() {
	var xhttp;
	if (window.XMLHttpRequest) {
    xhttp = new XMLHttpRequest();
    } else {
    xhttp = new ActiveXObject("Microsoft.XMLHTTP");
	}
	xhttp.onreadystatechange = function() {
    if (this.readyState == 4 && this.status == 200) {
      document.getElementsByClassName("josip")[0].innerHTML = this.responseText;
    }
	};
	xhttp.open("GET", "ajax_slika1.html", true);
	xhttp.send();
	}
	</script>
		
		<p>
		<a class="simple-ajax-popup-align-top" href='luka.html'>
		<img class="featurette-image img-responsive center-block img-rounded" border="0" alt="LUKA" src="img/luka_mala.jpg" width="150" height="150">
		</a>
		<div class="luka">
		<center>
		<h3>		
		<a class="luka" onclick="Ajaxluka();this.style.visibility= 'hidden';" role="button" data-clipboard-text="Kako položiti" data-toggle="tooltip" data-placement="bottom">LUKA</a></h3>
		</center>
		</p>
		</div>
		
	<script>
	function Ajaxluka() {
	var xhttp;
	if (window.XMLHttpRequest) {
    xhttp = new XMLHttpRequest();
    } else {
    xhttp = new ActiveXObject("Microsoft.XMLHTTP");
	}
	xhttp.onreadystatechange = function() {
    if (this.readyState == 4 && this.status == 200) {
      document.getElementsByClassName("luka")[0].innerHTML = this.responseText;
    }
	};
	xhttp.open("GET", "ajax_luka.html", true);
	xhttp.send();
	}
	</script>
		
			</div>
		</div>
		<hr class="featurette-divider">
		
		 <div class="linkovi"id="aj">
		 <center>
              <h4><a class="btn btn-lg btn-primary" href="http://www.etfos.unios.hr/">FERIT</a>
		<a class="btn btn-lg btn-primary" href="http://www.etfos.unios.hr/studiji/strucni-studij/SR601/7#anc">Multimedijska tehnika SR601</a>
			 
		<a class="btn btn-lg btn-primary" href="Anin kolegij.html">Lukin najbolji kolegij</a>
		<a class="btn btn-lg btn-primary" href="Petrov kolegij.html"> Josipov omiljeni kolegij</a>
			  </h4></center>
			  
			  
            </div>
			 
		
        
      


      <!-- Site footer -->
      <footer class="footer">
        <p>Stranica je izrađena za vrijeme pohađanja
		Fakulteta elektrotehnike, računarstva i informacijskih tehnologija u Osijeku u sklopu kolegija
		Multimedijska tehnika</p>
      </footer>

    </div> <!-- /container -->


	<script type="text/javascript">
      $(document).ready(function() {

        $('.simple-ajax-popup-align-top').magnificPopup({
          type: 'ajax',
          alignTop: true,
          overflowY: 'scroll' // as we know that popup content is tall we set scroll overflow by default to avoid jump
        });

        $('.simple-ajax-popup').magnificPopup({
          type: 'ajax'
        });
        
      });
    </script>   
	
  </body>
</html>
