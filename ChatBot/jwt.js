'use strict';

const jsonwebtoken = require('jsonwebtoken');

const jwt = jsonwebtoken.sign({scope: 'app'}, 'vUaUCJkAlBnu31RyiSlgU7oyoUq_b9-z2UlL4j0o9s9KevN0PNE8SJchl8nx5fSPf9HtMU2CzpYKTVQrxBXCJQ', { header : { kid : "app_5d199282c406f60012d950d5" }} );

module.exports = jwt;

// If run directly, print JWT to cmd line 
if (process.argv[1] === __filename) {
    console.log(jwt);
}