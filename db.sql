CREATE DATABASE vegan_restaurants;
\c vegan_restaurants;
DROP TABLE IF EXISTS Restaurant CASCADE;
DROP TABLE IF EXISTS City CASCADE;

CREATE TABLE City
(
    id   SERIAL PRIMARY KEY,
    name VARCHAR(255) UNIQUE
);

CREATE TABLE Restaurant
(
    id            SERIAL PRIMARY KEY,
    name          VARCHAR(255)   NOT NULL UNIQUE,
    address       VARCHAR(255)   NOT NULL,
    cityId        INT            NOT NULL,
    zipcode       INT            NOT NULL,
    latitude      DECIMAL(10, 8) NOT NULL,
    longitude     DECIMAL(10, 8) NOT NULL,
    telephone         VARCHAR(30),
    opening_hours VARCHAR(255),
    delivery      BOOLEAN        NOT NULL,
    FOREIGN KEY (cityId) REFERENCES City ON DELETE CASCADE 
);

DROP TABLE IF EXISTS Link CASCADE;
CREATE TABLE Link
(
    id   SERIAL PRIMARY KEY,
    type VARCHAR(50) UNIQUE
);

DROP TABLE IF EXISTS RestaurantLink CASCADE;
CREATE TABLE RestaurantLink
(
    restaurantId INT,
    linkType     INT,
    link         VARCHAR(255),
    PRIMARY KEY (restaurantId, linkType),
    FOREIGN KEY (restaurantId) REFERENCES Restaurant ON DELETE CASCADE ,
    FOREIGN KEY (linkType) REFERENCES Link ON DELETE CASCADE 
);

-- CREATE OR REPLACE VIEW FilteredRestaurant AS SELECT * FROM Restaurant;
-- CREATE OR REPLACE VIEW FilteredLink AS SELECT * FROM Link;
-- CREATE OR REPLACE VIEW FilteredRestaurantLink AS SELECT * FROM RestaurantLink;

DROP TYPE IF EXISTS linkExportType;
CREATE TYPE linkExportType AS
(
    type VARCHAR(50),
    link VARCHAR(255)
);

DROP TYPE IF EXISTS restaurantExportType;
CREATE TYPE restaurantExportType AS
(
    id            INT,
    name          VARCHAR(255),
    address       VARCHAR(255),
    zipcode       INT,
    latitude      DECIMAL(10, 8),
    longitude     DECIMAL(10, 8),
    telephone         VARCHAR(30),
    opening_hours VARCHAR(255),
    delivery      BOOLEAN,
    city          VARCHAR(30),
    websiteLinks  jsonb
);

INSERT INTO City(name)
VALUES ('Zagreb'),
       ('Split'),
       ('Zadar'),
       ('Dubrovnik'),
       ('Bol');

CREATE OR REPLACE FUNCTION CityNameToId(cityName text)
    RETURNS INTEGER
    LANGUAGE plpgsql AS
$$
DECLARE
    cityId integer;
BEGIN
    SELECT id INTO cityId FROM city WHERE name = cityName;
    return cityId;
END
$$;
DO
LANGUAGE plpgsql
$$
    DECLARE
        zagrebId    integer;
        splitId     integer;
        zadarId     integer;
        dubrovnikId integer;
        bolId       integer;
    BEGIN
        SELECT CityNameToId('Zagreb') INTO zagrebId;
        SELECT CityNameToId('Split') INTO splitId;
        SELECT CityNameToId('Zadar') INTO zadarId;
        SELECT CityNameToId('Dubrovnik') INTO dubrovnikId;
        SELECT CityNameToId('Bol') INTO bolId;
        INSERT INTO Restaurant(name, address, cityId, zipcode, latitude, longitude, telephone, opening_hours, delivery)
        VALUES ('Restoran Vegehop', 'Vlaška ulica 79', zagrebId, 10000, 45.814623428550874, 15.988238793652105,
                '014649400', 'Mon-Sat 12:00-20:00', true),
               ('Gajbica', 'Vlaška ulica 7', zagrebId, 10000, 45.813914984651724, 15.979462714908617, '0915141274',
                'Mon-Sat 11:00-18:00', true),
               ('OAZA Joyful Kitchen', 'Ulica Pavla Radića 9', zagrebId, 10000, 45.81434540170935, 15.97565003993682,
                '0976602744', 'Mon-Fri 10:00-21:00,Sat 12:00-21:00', true),
               ('BEKIND', 'Ilica 75', zagrebId, 10000, 45.812453447837015, 15.964507144624557, '015534763',
                'Tue-Sat 09:00-22:00,Sun 09:00-16:00', false),
               ('Zrno bio bistro', 'Medulićeva ulica 20', zagrebId, 10000, 45.8112211302705, 15.966675980193918,
                '014847540', 'Mon-Sat 12:00-21:30', true),
               ('Falafel, etc.', 'Ulica Andrije Žaje 60', zagrebId, 10000, 45.806958903578774, 15.953340244904695,
                '012343945', 'Mon-Sun 11:00-21:30', true),
               ('Shambala', 'Iločka ulica 34', zagrebId, 10000, 45.79956602909583, 15.959670741691376, '0957618710',
                'Mon-Fri 11:00-18:00,Sat-Sun 11:00:16:00', true),
               ('Simple Green by Jelena', 'Zelinska Ulica 7', zagrebId, 10000, 45.801063543477234, 15.972466341691469,
                '015561679', 'Mon-Sat 08:00-16:00', true),
               ('Vege Fino za sve', 'Ulica Lavoslava Ružičke 48', zagrebId, 10000, 45.79626823583559,
                15.969863328201175, '098777577', 'Mon-Fri 10:00-18:00,Sun 12:00-18:00', true),
               ('Barcode Mitra', 'Zagrebačka cesta 113', zagrebId, 10000, 45.806738490613796, 15.924704642365715,
                '013770428', 'Mon-Sat 14:00-22:00', true),
               ('Pandora Greenbox', 'Obrov ulica 4', splitId, 21000, 43.509304163256076, 16.437649495980676,
                '021236120', 'Mon-Sun 08:30-12:00', false),
               ('Upcafe', 'Ulica Domovinskog rata 29a', splitId, 21000, 43.5169287239683, 16.44540914498057,
                '0916210500', 'Mon-Sat 07:00-20:00,Sun 08:00-20:00', true),
               ('The Botanist', 'Ulica Mihovila Pavlinovića 4', zadarId, 23000, 44.116437403147856, 15.225939316140703,
                '0924232296', 'Mon-Sun 12:00-23:00', false),
               ('Nishta', 'Prijeko ulica bb', dubrovnikId, 20000, 42.675349718317385, 18.10592518443174, '020322088',
                'Mon-Sat 11:30-22:00', false),
               ('BioMania Bistro Bol', 'Rudina 10', bolId, 21420, 43.26175995831936, 16.65493408371649, '0919362276',
                'Mon-Thu 13:00-21:00,Sat 13:00-21:00,Sun 10:00-22:00', false);
    END;
$$;

INSERT INTO Link(type)
VALUES ('website'),
       ('facebook'),
       ('instagram'),
       ('youtube'),
       ('tiktok'),
       ('linkedin'),
       ('tripadvisor'),
       ('x');

CREATE OR REPLACE FUNCTION LinkTypeToId(linkType text)
    RETURNS INTEGER
    LANGUAGE plpgsql AS
$$
DECLARE
    linkId integer;
BEGIN
    SELECT id INTO linkId FROM Link WHERE type = linkType;
    return linkId;
END
$$;

DO LANGUAGE plpgsql
$$
    DECLARE
        website     int;
        facebook    int;
        instagram   int;
        youtube     int;
        tiktok      int;
        linkedin    int;
        tripadvisor int;
        x           int;
    BEGIN
        SELECT LinkTypeToId('website') INTO website;
        SELECT LinkTypeToId('facebook') INTO facebook;
        SELECT LinkTypeToId('instagram') INTO instagram;
        SELECT LinkTypeToId('youtube') INTO youtube;
        SELECT LinkTypeToId('tiktok') INTO tiktok;
        SELECT LinkTypeToId('linkedin') INTO linkedin;
        SELECT LinkTypeToId('tripadvisor') INTO tripadvisor;
        SELECT LinkTypeToId('x') INTO x;
        INSERT INTO RestaurantLink(restaurantId, linkType, link)
        VALUES (1, website, 'vegehop.hr'),
               (1, facebook, 'facebook.com/vegehop'),
               (1, instagram, 'instagram.com/vegehop_restaurant'),
               (1, youtube, 'youtube.com/user/Vegehop'),
               (2, website, 'gajbica.eatbu.com'),
               (2, facebook, 'facebook.com/gajbicahr'),
               (2, instagram, 'instagram.com/gajbica_zg'),
               (3, website, 'joyful-kitchen.com'),
               (3, facebook, 'facebook.com/oaza.joyful.kitchen'),
               (3, instagram, 'instagram.com/oaza.joyful.kitchen'),
               (3, tiktok, 'tiktok.com/@oaza.joyful.kitchen'),
               (3, linkedin, 'linkedin.com/company/oaza-joyful-kitchen'),
               (4, website, 'bekind.hr'),
               (4, facebook, 'facebook.com/bekind.zagreb'),
               (4, instagram, 'instagram.com/bekind.zagreb'),
               (5, website, 'zrnobiobistro.hr'),
               (5, facebook, 'facebook.com/zrnobiobistro'),
               (5, instagram, 'instagram.com/zrnobiobistro'),
               (6, website, 'falafeletc.com.hr'),
               (6, facebook, 'facebook.com/falafeletczg'),
               (7, facebook, 'facebook.com/people/Shambala-Vegan-food-sweets-more/100083364727743'),
               (8, website, 'simplegreenbyjelena.com'),
               (8, facebook, 'facebook.com/profile.php?id=100057104513129'),
               (8, instagram, 'instagram.com/simplegreen_by_jelena'),
               (8, tripadvisor,
                'tripadvisor.com/Restaurant_Review-g294454-d11844934-Reviews-Simple_Green_Bake_by_Jelena-Zagreb_Central_Croatia.html'),
               (9, website, 'vegefinozasve.com'),
               (9, facebook, 'facebook.com/VegeFinoZaSve'),
               (10, website, 'barcodemitra.hr'),
               (10, facebook, 'facebook.com/BarcodeMitra'),
               (10, instagram, 'instagram.com/barcode_mitra'),
               (11, website, 'pandoragreenbox.com'),
               (11, facebook, 'facebook.com/PandoraGreenbox'),
               (11, instagram, 'instagram.com/pandora_greenbox'),
               (12, website, 'upcafe.hr'),
               (12, facebook, 'facebook.com/upcafe12'),
               (12, instagram, 'instagram.com/upcafe.split'),
               (12, tripadvisor,
                'tripadvisor.com/Restaurant_Review-g295370-d5773474-Reviews-UPcafe-Split_Split_Dalmatia_County_Dalmatia.html'),
               (13, instagram, 'instagram.com/botanist_zadar'),
               (14, website, 'nishtarestaurant.com'),
               (14, facebook, 'facebook.com/nishtarestaurant'),
               (14, instagram, 'instagram.com/explore/locations/798999121'),
               (14, x, 'twitter.com/hashtag/nishta'),
               (15, website, 'biomania.hr'),
               (15, facebook, 'facebook.com/biomaniahr'),
               (15, instagram, 'instagram.com/biomaniahr'),
               (15, tripadvisor,
                'tripadvisor.com/Restaurant_Review-g303802-d14997286-Reviews-BioMania_Street_Food-Bol_Brac_Island_Split_Dalmatia_County_Dalmatia.html');
    END;
$$;

--restaurant view that can be filtered
CREATE OR REPLACE VIEW RestaurantView AS
(
SELECT r.id,
       r.name,
       address,
       cityId,
       zipcode,
       latitude,
       longitude,
       telephone,
       opening_hours,
       delivery,
       City.name as city
FROM Restaurant r
         JOIN City ON r.cityId = City.id
ORDER BY r.id
    );

--link view that can be filtered
CREATE OR REPLACE VIEW LinkView AS
(
SELECT restaurantId, type, link
FROM RestaurantLink
         JOIN Link ON RestaurantLink.linkType = Link.id
    );

--view that displays human readable data about the + website links as a JSON type
CREATE OR REPLACE VIEW RestaurantWithLinksView AS
(
SELECT *,
       (SELECT jsonb_agg(websiteLinkType)
        FROM (SELECT CAST((linkMerge.type, linkMerge.link) AS linkExportType) as websiteLinkType
              FROM (SELECT LinkView.type, LinkView.link
                    FROM LinkView
                    WHERE restaurantId = RestaurantView.id) linkMerge) as websiteLinkQuery) as websiteLinks
FROM RestaurantView
    );

--view that casts RestaurantView into restaurantExportType so that it can be processed into JSON
CREATE OR REPLACE VIEW RestaurantCastView AS
(
SELECT CAST((id, name, address, zipcode, latitude, longitude, telephone, opening_hours, delivery, city,
             websiteLinks) as restaurantExportType) as RestaurantCast
FROM RestaurantWithLinksView
    );

--view that shows data of a restaurant as JSON
CREATE OR REPLACE VIEW JsonRestaurantsView AS
(
SELECT TO_JSONB(RestaurantCast) AS jsonRestaurant
FROM RestaurantCastView
    );

--view that shows data of all restaurant as a single JSON field
CREATE OR REPLACE VIEW JsonExportView AS
(
SELECT JSONB_AGG(jsonRestaurant) AS jsonbAgg
FROM JsonRestaurantsView
    );

--a view used to export data to a CSV file
CREATE OR REPLACE VIEW RestaurantCsvView AS
(
SELECT r.id,
       r.name,
       address,
       zipcode,
       latitude,
       longitude,
       telephone,
       opening_hours,
       delivery,
       r.city as city,
       l.type as linkType,
       rl.link
FROM RestaurantView r
         JOIN RestaurantLink rl ON r.id = rl.restaurantId
         JOIN Link l ON rl.linkType = l.id
ORDER BY r.id, l.id
    );
