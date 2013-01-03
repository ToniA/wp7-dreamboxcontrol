This is a simple remote use program for Enigma2-based set-top-boxes, like the Dreambox DM7025.

At the moment the whole GUI is only in Finnish, although all the comments etc. in the code are in English

The features are:
* runs on Windows Phone 'Mango' (and newer), tested on Nokia Lumia 800
* connects to the Dreambox over SSH, using the SSH.NET library
* supports connection through another box, like a router running DD-WRT, tested on Linksys WRT-54GL
   * this box only needs to have a working 'wget' program
* Dreambox features
   * display the 'now running' from the favourite bouquet
      * display the EPG of the selected channel
	  * set timers into EPG events
   * display and delete timers
   * display and delete recordings

Configuration guidance
* "Dreamboxin IP-osoite" - the IP address of your Dreambox in your home network, like 192.168.0.2
* "SSH-palvelin" - the IP address of your SSH server. My WRT-54GL runs the dyndns client, so the address can be like 'myhome.dyndns.org'
* "SSH-palvelimen portti" - the port number of the SSH server
* "SSH-tili" - the SSH account, like 'root'
* "SSH-tilin salasana" - the password of the SSH account (if you choose to use the password)
* "SSH-tilin avain" - the private SSH key (if you choose to use key-based authentication)