const pg = require("pg");
const fs = require("fs");
const CommandHelper = require("./CommandHelper");
const commandHelper = new CommandHelper()
let dbConnString
let exportCommandString
fs.readFile('connectionString.txt', 'utf8', (err, data) => dbConnString = data)
fs.readFile('commandString.txt', 'utf8', (err, data) => exportCommandString = data)

class Db{
    static async updateViews(name, address, city, zipcode, latitude, longitude, phone, opening_hours, delivery, linkType, link){
        console.log(dbConnString)
        const client = new pg.Client(dbConnString)
        client.connect()

        let createLinkViewQuery = `CREATE OR REPLACE VIEW LinkView AS(SELECT restaurantId, type, link 
        FROM RestaurantLink JOIN Link ON RestaurantLink.linkType = Link.id 
        WHERE type LIKE '%' || '${linkType ?? ''}' || '%' AND link LIKE '%' || '${link ?? ''}' || '%')`
        await client.query(createLinkViewQuery)

        let createWebsiteViewQuery = `CREATE OR REPLACE VIEW RestaurantView AS(
        SELECT r.id, r.name, address, cityId, zipcode, latitude, longitude, phone, opening_hours, delivery, City.name as city 
        FROM Restaurant r JOIN City ON r.cityId = City.id
        WHERE LOWER(r.name) LIKE LOWER('%' || '${name ?? ''}' || '%') AND
        LOWER(address) LIKE LOWER('%' || '${address ?? ''}' || '%') AND
        LOWER(City.name) LIKE LOWER('%' || '${city ?? ''}' || '%') AND
        ${zipcode === null || zipcode === undefined ? '' : `zipcode = ${zipcode} AND`}
        ${latitude === null || zipcode === undefined ? '' : `latitude = ${latitude} AND`}
        ${longitude === null || zipcode === undefined ? '' : `longitude = ${longitude} AND`}
        ${delivery === null || zipcode === undefined ? '' : `delivery = ${delivery} AND`}
        LOWER(phone) LIKE LOWER('%' || '${phone ?? ''}' || '%') AND
        LOWER(opening_hours) LIKE LOWER('%' || '${opening_hours ?? ''}' || '%')
        ORDER BY r.id);`
        console.log(createWebsiteViewQuery)
        await client.query(createWebsiteViewQuery)
    }

    static async exportFiles(name, address, city, zipcode, latitude, longitude, phone, opening_hours, delivery, linkType, link){
        console.log(exportCommandString)
        await Db.updateViews(name, address, city, zipcode, latitude, longitude, phone, opening_hours, delivery, linkType, link)
        let jsonExportCommand = `${exportCommandString} -c "\\copy (SELECT jsonbAgg FROM JsonExportView) TO /tmp/veganRestaurants.json;"`
        let csvExportCommand = `${exportCommandString} -c "\\copy (SELECT * FROM RestaurantCsvView) TO '/tmp/veganRestaurants.csv' DELIMITER ';' CSV HEADER;"`
        await commandHelper.executeProcess(jsonExportCommand)
        await commandHelper.executeProcess(csvExportCommand)
        await setTimeout(() => Db.updateViews(), 250)
    }
}

module.exports = Db