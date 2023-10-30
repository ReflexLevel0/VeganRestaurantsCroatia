--
-- PostgreSQL database dump
--

-- Dumped from database version 15.4
-- Dumped by pg_dump version 15.4

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- Name: linkexporttype; Type: TYPE; Schema: public; Owner: postgres
--

CREATE TYPE public.linkexporttype AS (
	type character varying(50),
	link character varying(255)
);


ALTER TYPE public.linkexporttype OWNER TO postgres;

--
-- Name: restaurantexporttype; Type: TYPE; Schema: public; Owner: postgres
--

CREATE TYPE public.restaurantexporttype AS (
	id integer,
	name character varying(255),
	address character varying(255),
	zipcode integer,
	latitude numeric(10,8),
	longitude numeric(10,8),
	phone character varying(30),
	opening_hours character varying(255),
	delivery boolean,
	city character varying(30),
	websitelinks jsonb
);


ALTER TYPE public.restaurantexporttype OWNER TO postgres;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: city; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.city (
    id integer NOT NULL,
    name character varying(255)
);


ALTER TABLE public.city OWNER TO postgres;

--
-- Name: link; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.link (
    id integer NOT NULL,
    type character varying(50)
);


ALTER TABLE public.link OWNER TO postgres;

--
-- Name: restaurant; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.restaurant (
    id integer NOT NULL,
    name character varying(255) NOT NULL,
    address character varying(255) NOT NULL,
    cityid integer NOT NULL,
    zipcode integer NOT NULL,
    latitude numeric(10,8) NOT NULL,
    longitude numeric(10,8) NOT NULL,
    phone character varying(30),
    opening_hours character varying(255),
    delivery boolean NOT NULL
);


ALTER TABLE public.restaurant OWNER TO postgres;

--
-- Name: restaurantlink; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.restaurantlink (
    restaurantid integer NOT NULL,
    linktype integer NOT NULL,
    link character varying(255)
);


ALTER TABLE public.restaurantlink OWNER TO postgres;

--
-- Name: restaurantwithlinksview; Type: VIEW; Schema: public; Owner: postgres
--

CREATE VIEW public.restaurantwithlinksview AS
 SELECT r.id,
    r.name,
    r.address,
    r.cityid,
    r.zipcode,
    r.latitude,
    r.longitude,
    r.phone,
    r.opening_hours,
    r.delivery,
    city.name AS city,
    ( SELECT jsonb_agg(websitelinkquery.websitelinktype) AS jsonb_agg
           FROM ( SELECT ROW(linkmerge.type, linkmerge.link)::public.linkexporttype AS websitelinktype
                   FROM ( SELECT link.type,
                            restaurantlink.link
                           FROM (public.restaurantlink
                             JOIN public.link ON ((restaurantlink.linktype = link.id)))
                          WHERE (restaurantlink.restaurantid = r.id)) linkmerge) websitelinkquery) AS websitelinks
   FROM (public.restaurant r
     JOIN public.city ON ((r.cityid = city.id)))
  ORDER BY r.id;


ALTER TABLE public.restaurantwithlinksview OWNER TO postgres;

--
-- Name: restaurantcastview; Type: VIEW; Schema: public; Owner: postgres
--

CREATE VIEW public.restaurantcastview AS
 SELECT ROW(restaurantwithlinksview.id, restaurantwithlinksview.name, restaurantwithlinksview.address, restaurantwithlinksview.zipcode, restaurantwithlinksview.latitude, restaurantwithlinksview.longitude, restaurantwithlinksview.phone, restaurantwithlinksview.opening_hours, restaurantwithlinksview.delivery, (restaurantwithlinksview.city)::character varying(30), restaurantwithlinksview.websitelinks)::public.restaurantexporttype AS restaurantcast
   FROM public.restaurantwithlinksview;


ALTER TABLE public.restaurantcastview OWNER TO postgres;

--
-- Name: jsonrestaurantsview; Type: VIEW; Schema: public; Owner: postgres
--

CREATE VIEW public.jsonrestaurantsview AS
 SELECT to_jsonb(restaurantcastview.restaurantcast) AS jsonrestaurant
   FROM public.restaurantcastview;


ALTER TABLE public.jsonrestaurantsview OWNER TO postgres;

--
-- Name: jsonexportview; Type: VIEW; Schema: public; Owner: postgres
--

CREATE VIEW public.jsonexportview AS
 SELECT jsonb_agg(jsonrestaurantsview.jsonrestaurant) AS jsonbagg
   FROM public.jsonrestaurantsview;


ALTER TABLE public.jsonexportview OWNER TO postgres;

--
-- Name: restaurantcsvview; Type: VIEW; Schema: public; Owner: postgres
--

CREATE VIEW public.restaurantcsvview AS
 SELECT r.id,
    r.name,
    r.address,
    r.zipcode,
    r.latitude,
    r.longitude,
    r.phone,
    r.opening_hours,
    r.delivery,
    c.name AS city,
    l.type AS linktype,
    rl.link
   FROM (((public.restaurant r
     JOIN public.city c ON ((r.cityid = c.id)))
     JOIN public.restaurantlink rl ON ((r.id = rl.restaurantid)))
     JOIN public.link l ON ((rl.linktype = l.id)))
  ORDER BY r.id, l.id;


ALTER TABLE public.restaurantcsvview OWNER TO postgres;

--
-- Data for Name: city; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.city (id, name) FROM stdin;
1	Zagreb
2	Split
3	Zadar
4	Dubrovnik
5	Bol
6	Pag
\.


--
-- Data for Name: link; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.link (id, type) FROM stdin;
1	website
2	facebook
3	instagram
4	youtube
5	tiktok
6	linkedin
7	tripadvisor
8	X
\.


--
-- Data for Name: restaurant; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.restaurant (id, name, address, cityid, zipcode, latitude, longitude, phone, opening_hours, delivery) FROM stdin;
1	Restoran Vegehop	Vlaška ulica 79	1	10000	45.81462343	15.98823879	014649400	Mon-Sat 12:00-20:00	t
2	Gajbica	Vlaška ulica 7	1	10000	45.81391498	15.97946271	0915141274	Mon-Sat 11:00-18:00	t
3	OAZA Joyful Kitchen	Ulica Pavla Radića 9	1	10000	45.81434540	15.97565004	0976602744	Mon-Fri 10:00-21:00,Sat 12:00-21:00	t
4	BEKIND	Ilica 75	1	10000	45.81245345	15.96450714	015534763	Tue-Sat 09:00-22:00,Sun 09:00-16:00	f
5	Zrno bio bistro	Medulićeva ulica 20	1	10000	45.81122113	15.96667598	014847540	Mon-Sat 12:00-21:30	t
6	Falafel, etc.	Ulica Andrije Žaje 60	1	10000	45.80695890	15.95334024	012343945	Mon-Sun 11:00-21:30	t
7	Shambala	Iločka ulica 34	1	10000	45.79956603	15.95967074	0957618710	Mon-Fri 11:00-18:00,Sat-Sun 11:00:16:00	t
8	Simple Green by Jelena	Zelinska Ulica 7	1	10000	45.80106354	15.97246634	015561679	Mon-Sat 08:00-16:00	t
9	Vege Fino za sve	Ulica Lavoslava Ružičke 48	1	10000	45.79626824	15.96986333	098777577	Mon-Fri 10:00-18:00,Sun 12:00-18:00	t
10	Barcode Mitra	Zagrebačka cesta 113	1	10000	45.80673849	15.92470464	013770428	Mon-Sat 14:00-22:00	t
11	Pandora Greenbox	Obrov ulica 4	2	21000	43.50930416	16.43764950	021236120	Mon-Sun 08:30-12:00	f
12	Upcafe	Ulica Domovinskog rata 29a	2	21000	43.51692872	16.44540914	0916210500	Mon-Sat 07:00-20:00,Sun 08:00-20:00	t
13	The Botanist	Ulica Mihovila Pavlinovića 4	3	23000	44.11643740	15.22593932	0924232296	Mon-Sun 12:00-23:00	f
14	Nishta	Prijeko ulica bb	4	20000	42.67534972	18.10592518	020322088	Mon-Sat 11:30-22:00	f
15	BioMania Bistro Bol	Rudina 10	5	21420	43.26175996	16.65493408	0919362276	Mon-Thu 13:00-21:00,Sat 13:00-21:00,Sun 10:00-22:00	f
\.


--
-- Data for Name: restaurantlink; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.restaurantlink (restaurantid, linktype, link) FROM stdin;
1	1	vegehop.hr
1	2	facebook.com/vegehop
1	3	instagram.com/vegehop_restaurant
1	4	youtube.com/user/Vegehop
2	1	gajbica.eatbu.com
2	2	facebook.com/gajbicahr
2	3	instagram.com/gajbica_zg
3	1	joyful-kitchen.com
3	2	facebook.com/oaza.joyful.kitchen
3	3	instagram.com/oaza.joyful.kitchen
3	5	tiktok.com/@oaza.joyful.kitchen
3	6	linkedin.com/company/oaza-joyful-kitchen
4	1	bekind.hr
4	2	facebook.com/bekind.zagreb
4	3	instagram.com/bekind.zagreb
5	1	zrnobiobistro.hr
5	2	facebook.com/zrnobiobistro
5	3	instagram.com/zrnobiobistro
6	1	falafeletc.com.hr
6	2	facebook.com/falafeletczg
7	2	facebook.com/people/Shambala-Vegan-food-sweets-more/100083364727743
8	1	simplegreenbyjelena.com
8	2	facebook.com/profile.php?id=100057104513129
8	3	instagram.com/simplegreen_by_jelena
8	7	tripadvisor.com/Restaurant_Review-g294454-d11844934-Reviews-Simple_Green_Bake_by_Jelena-Zagreb_Central_Croatia.html
9	1	vegefinozasve.com
9	2	facebook.com/VegeFinoZaSve
10	1	barcodemitra.hr
10	2	facebook.com/BarcodeMitra
10	3	instagram.com/barcode_mitra
11	1	pandoragreenbox.com
11	2	facebook.com/PandoraGreenbox
11	3	instagram.com/pandora_greenbox
12	1	upcafe.hr
12	2	facebook.com/upcafe12
12	3	instagram.com/upcafe.split
12	7	tripadvisor.com/Restaurant_Review-g295370-d5773474-Reviews-UPcafe-Split_Split_Dalmatia_County_Dalmatia.html
13	3	instagram.com/botanist_zadar
14	1	nishtarestaurant.com
14	2	facebook.com/nishtarestaurant
14	3	instagram.com/explore/locations/798999121
14	8	twitter.com/hashtag/nishta
15	1	biomania.hr
15	2	facebook.com/biomaniahr
15	3	instagram.com/biomaniahr
15	7	tripadvisor.com/Restaurant_Review-g303802-d14997286-Reviews-BioMania_Street_Food-Bol_Brac_Island_Split_Dalmatia_County_Dalmatia.html
\.


--
-- Name: city city_name_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.city
    ADD CONSTRAINT city_name_key UNIQUE (name);


--
-- Name: city city_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.city
    ADD CONSTRAINT city_pkey PRIMARY KEY (id);


--
-- Name: link link_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.link
    ADD CONSTRAINT link_pkey PRIMARY KEY (id);


--
-- Name: restaurant restaurant_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.restaurant
    ADD CONSTRAINT restaurant_pkey PRIMARY KEY (id);


--
-- Name: restaurantlink restaurantlink_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.restaurantlink
    ADD CONSTRAINT restaurantlink_pkey PRIMARY KEY (restaurantid, linktype);


--
-- Name: restaurant restaurant_cityid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.restaurant
    ADD CONSTRAINT restaurant_cityid_fkey FOREIGN KEY (cityid) REFERENCES public.city(id);


--
-- Name: restaurantlink restaurantlink_linktype_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.restaurantlink
    ADD CONSTRAINT restaurantlink_linktype_fkey FOREIGN KEY (linktype) REFERENCES public.link(id);


--
-- Name: restaurantlink restaurantlink_restaurantid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.restaurantlink
    ADD CONSTRAINT restaurantlink_restaurantid_fkey FOREIGN KEY (restaurantid) REFERENCES public.restaurant(id);


--
-- PostgreSQL database dump complete
--

