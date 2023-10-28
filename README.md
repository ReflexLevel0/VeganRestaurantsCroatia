# VeganRestaurantsCroatia
This repository contains data about vegan restaurants all over croatia. The datasets can be accessed both in CSV and JSON formats.

# Dataset description

| Name  | Description |
| --- | --- |
| Title  | Vegan restaurants in Croatia  |
| Description | Information about vegan restaurants in Croatia |
| Keywords | vegan,restaurant,croatia |
| Last update | 2023-10-28 |
| Language | English |
| Dataset version | 0.1 |
| License | Creative Commons Zero |
| Media type | text |
| Maintenance status | maintained |
| Data fields | id,name,address,zipcode,langitude,longitude,phone,website,opening_hours,delivery,cityName |
| Author | Dominik Dejanović |

# Dataset version history

| Version | Date | Change description |
| --- | --- | --- |
| 0.1 | 2023-10-28 | created initial repository with no data |

# CSV file structure

Delimiter: ";"

| Field | Description | Datatype | Primary key | Required |
| --- | --- | --- | --- | --- |
| id | Unique identifier of the restaurant | numeric | true | true |
| name | Name of the restaurant | string | false | true |
| address | Current location of the restaurant | string | false | true |
| zipcode | Restaurant zipcode location | numberic | false | true |
| latitude | Latitude coordinates of the restaurant | numeric | false | true |
| longitude | Longitude coordinates of the restaurant | numeric | false | true |
| phone | Phone number used to communicate with the restaurant | string | false | false |
| website | Website link used by the restaurant | string | false | false |
| opening_hours | Opening and closing hours by day, separated by "," (example: "Mon-Fri 10:00-18:00,Sun 10:00-16:00") | string | false | false |
| delivery | Whether or not the restaurant supports delivery ("f" for false and "t" for true) | string | false | true |
| cityName | Name of the city in which the restaurant is in | string | false | true |
