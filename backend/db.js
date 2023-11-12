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
        const client = new pg.Client(dbConnString)
        client.connect()

        let hasLinkType = linkType === null || linkType === undefined
        let hasLink = link === null || link === undefined
        let hasName = name === null || name === undefined
        let hasAddress = address === null || address === undefined
        let hasCity = city === null || city === undefined
        let hasZipcode = zipcode === null || zipcode === undefined
        let hasLatitude = latitude === null || latitude === undefined
        let hasLongitude = longitude === null || longitude === undefined
        let hasDelivery = delivery === null || delivery === undefined
        let hasPhone = phone === null || phone === undefined
        let hasOpeningHours = opening_hours === null || opening_hours === undefined

        let linkViewWhere = `
        ${hasLinkType ? '' : `LOWER(type) LIKE LOWER('%' || '${linkType}' || '%') OR`}
        ${hasLink ? '' : `LOWER(link) LIKE LOWER('%' || '${link}' || '%')`}`
        linkViewWhere = linkViewWhere.replace('\\n', '')
        linkViewWhere = linkViewWhere.trim()
        if(linkViewWhere.endsWith('OR')) linkViewWhere = linkViewWhere.substring(0, linkViewWhere.length - 2)
        linkViewWhere = linkViewWhere === '' ? '' : `WHERE ${linkViewWhere}`

        let createLinkViewQuery = `CREATE OR REPLACE VIEW LinkView AS(SELECT restaurantId, type, link 
        FROM RestaurantLink JOIN Link ON RestaurantLink.linkType = Link.id 
        ${linkViewWhere})`
        await client.query(createLinkViewQuery)
        let restaurantViewWhere = `
        ${hasName ? '' : `LOWER(r.name) LIKE LOWER('%' || '${name}' || '%') OR`}
        ${hasAddress ? '' : `LOWER(address) LIKE LOWER('%' || '${address}' || '%') OR`}
        ${hasCity ? '' : `LOWER(City.name) LIKE LOWER('%' || '${city}' || '%') OR`}
        ${hasZipcode ? '' : `LOWER(zipcode::varchar(256)) LIKE LOWER('%' || '${zipcode}' || '%') OR`}
        ${hasLatitude ? '' : `LOWER(latitude::varchar(256)) LIKE LOWER('%' || '${latitude}' || '%') OR`}
        ${hasLongitude ? '' : `LOWER(longitude::varchar(256)) LIKE LOWER('%' || '${longitude}' || '%') OR`}
        ${hasDelivery ? '' : `LOWER(delivery::varchar(256)) LIKE LOWER('%' || '${delivery}' || '%') OR`}
        ${hasPhone ? '' : `LOWER(phone) LIKE LOWER('%' || '${phone}' || '%') OR`}
        ${hasOpeningHours ? '' : `LOWER(opening_hours) LIKE LOWER('%' || '${opening_hours}' || '%')`}`

        restaurantViewWhere = restaurantViewWhere.replace('\\n', ' ')
        restaurantViewWhere = restaurantViewWhere.trim()
        if(restaurantViewWhere.endsWith('OR')) restaurantViewWhere = restaurantViewWhere.substring(0, restaurantViewWhere.length - 2)
        restaurantViewWhere = restaurantViewWhere === '' ? '' : `WHERE ${restaurantViewWhere}`

        let createRestaurantViewQuery = `CREATE OR REPLACE VIEW RestaurantView AS(
        SELECT r.id, r.name, address, cityId, zipcode, latitude, longitude, phone, opening_hours, delivery, City.name as city 
        FROM Restaurant r JOIN City ON r.cityId = City.id
        ${restaurantViewWhere}
        ORDER BY r.id);`
        await client.query(createRestaurantViewQuery)
    }

    static async exportFiles(name, address, city, zipcode, latitude, longitude, phone, opening_hours, delivery, linkType, link){
        await Db.updateViews(name, address, city, zipcode, latitude, longitude, phone, opening_hours, delivery, linkType, link)
        let jsonExportCommand = `${exportCommandString} -c "\\copy (SELECT jsonbAgg FROM JsonExportView) TO /tmp/veganRestaurants.json;"`
        let csvExportCommand = `${exportCommandString} -c "\\copy (SELECT * FROM RestaurantCsvView) TO '/tmp/veganRestaurants.csv' DELIMITER ';' CSV HEADER;"`
        await commandHelper.executeProcess(jsonExportCommand)
        await commandHelper.executeProcess(csvExportCommand)
        await setTimeout(() => Db.updateViews(), 250)
    }
}

module.exports = Db