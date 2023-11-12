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
    const name = urlParams.get('name')
    const address = urlParams.get('')
    const city = urlParams.get('city')
    const zipcode = urlParams.get('zipcode')
    const latitude = urlParams.get('latitude')
    const longitude = urlParams.get('longitude')
    const phone = urlParams.get('phone')
    const opening_hours = urlParams.get('opening_hours')
    const delivery = urlParams.get('delivery')
    const linkType = urlParams.get('linkType')
    const link = urlParams.get('link')
    db.exportFiles(name, address, city, zipcode, latitude, longitude, phone, opening_hours, delivery, linkType, link).then(() => {
        let jsonData = null
        setTimeout(() => {
            fs.readFile('/tmp/veganRestaurants.json', 'utf-8', (err, data) => {
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