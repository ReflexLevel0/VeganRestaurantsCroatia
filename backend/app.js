const express = require('express');
const cors = require('cors')
const fs = require("fs");
const app = express();
const port = 3000;
const db = require('./db')

let corsOptions = {
    origin: ['http://localhost:8080'],
}
app.use(cors(corsOptions))

app.get('/', (req, res) => {
    const urlParams = new URLSearchParams(req.url.split("?")[1])
    const all = urlParams.get('all')
    let name, address, city, zipcode, latitude, longitude, phone, opening_hours, delivery, linkType, link
    if(all){
        name = all
        address = all
        city = all
        zipcode = all
        latitude = all
        longitude = all
        phone = all
        opening_hours = all
        delivery = all
        linkType = all
        link = all
    }else{
        name = urlParams.get('name')
        address = urlParams.get('address')
        city = urlParams.get('city')
        zipcode = urlParams.get('zipcode')
        latitude = urlParams.get('latitude')
        longitude = urlParams.get('longitude')
        phone = urlParams.get('phone')
        opening_hours = urlParams.get('opening_hours')
        delivery = urlParams.get('delivery')
        linkType = urlParams.get('linkType')
        link = urlParams.get('link')
    }
    db.exportFiles(name, address, city, zipcode, latitude, longitude, phone, opening_hours, delivery, linkType, link).then(() => {
        let jsonData = null
        setTimeout(() => {
            fs.readFile('/tmp/veganRestaurants.json', 'utf-8', (err, data) => {
                if(data === '\\N\n') data = '[]'
                jsonData = data
                res.send(jsonData)
            })
        }, 250)
    })
});

app.get('/json', (req, res) => res.sendFile('/tmp/veganRestaurants.json'))
app.get('/csv', (req, res) => res.sendFile('/tmp/veganRestaurants.csv'))

app.listen(port, () => {
    console.log(`Server listening at http://localhost:${port}`);
});