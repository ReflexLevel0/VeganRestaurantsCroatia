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
-- Name: jsonrestaurantType; Type: TYPE; Schema: public; Owner: postgres
--

CREATE TYPE public."jsonrestaurantType" AS (
	id integer,
	name character varying(255),
	address character varying(255),
	zipcode integer,
	latitude numeric(10,8),
	longitude numeric(10,8),
	phone character varying(30),
	website character varying(255),
	opening_hours character varying(255),
	delivery boolean,
	cityname character varying(30)
);


ALTER TYPE public."jsonrestaurantType" OWNER TO postgres;

--
-- Name: jsonrestauranttype; Type: TYPE; Schema: public; Owner: postgres
--

CREATE TYPE public.jsonrestauranttype AS (
	id integer,
	name character varying(255),
	address character varying(255),
	zipcode integer,
	latitude numeric(10,8),
	longitude numeric(10,8),
	phone character varying(30),
	website character varying(255),
	opening_hours character varying(255),
	delivery boolean,
	cityname character varying(30)
);


ALTER TYPE public.jsonrestauranttype OWNER TO postgres;

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
	website character varying(255),
	opening_hours character varying(255),
	delivery boolean,
	cityname character varying(30)
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
    website character varying(255),
    opening_hours character varying(255),
    delivery boolean NOT NULL
);


ALTER TABLE public.restaurant OWNER TO postgres;

--
-- Name: restaurantview; Type: VIEW; Schema: public; Owner: postgres
--

CREATE VIEW public.restaurantview AS
 SELECT restaurant.id,
    restaurant.name AS restaurantname,
    restaurant.address,
    restaurant.zipcode,
    restaurant.latitude,
    restaurant.longitude,
    restaurant.phone,
    restaurant.website,
    restaurant.opening_hours,
    restaurant.delivery,
    city.name AS cityname
   FROM (public.restaurant
     JOIN public.city ON ((restaurant.cityid = city.id)))
  ORDER BY restaurant.id;


ALTER TABLE public.restaurantview OWNER TO postgres;

--
-- Name: restaurantexportview; Type: VIEW; Schema: public; Owner: postgres
--

CREATE VIEW public.restaurantexportview AS
 SELECT ROW(restaurantview.id, restaurantview.restaurantname, restaurantview.address, restaurantview.zipcode, restaurantview.latitude, restaurantview.longitude, restaurantview.phone, restaurantview.website, restaurantview.opening_hours, restaurantview.delivery, (restaurantview.cityname)::character varying(30))::public.restaurantexporttype AS restaurantexport
   FROM public.restaurantview;


ALTER TABLE public.restaurantexportview OWNER TO postgres;

--
-- Name: jsonrestaurantsview; Type: VIEW; Schema: public; Owner: postgres
--

CREATE VIEW public.jsonrestaurantsview AS
 SELECT to_jsonb(restaurantexportview.restaurantexport) AS jsonrestaurant
   FROM public.restaurantexportview;


ALTER TABLE public.jsonrestaurantsview OWNER TO postgres;

--
-- Name: jsonexportview; Type: VIEW; Schema: public; Owner: postgres
--

CREATE VIEW public.jsonexportview AS
 SELECT jsonb_agg(jsonrestaurantsview.jsonrestaurant) AS jsonbagg
   FROM public.jsonrestaurantsview;


ALTER TABLE public.jsonexportview OWNER TO postgres;

--
-- Name: restaurant_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.restaurant_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.restaurant_id_seq OWNER TO postgres;

--
-- Name: restaurant_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.restaurant_id_seq OWNED BY public.restaurant.id;


--
-- Name: restaurant id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.restaurant ALTER COLUMN id SET DEFAULT nextval('public.restaurant_id_seq'::regclass);


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
-- Data for Name: restaurant; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.restaurant (id, name, address, cityid, zipcode, latitude, longitude, phone, website, opening_hours, delivery) FROM stdin;
1	Restoran Vegehop	Vlaška ulica 79	1	10000	45.81462343	15.98823879	014649400	vegehop.hr	Mon-Sat 12:00-20:00	t
2	Gajbica	Vlaška ulica 7	1	10000	45.81391498	15.97946271	0915141274	gajbica.eatbu.com	Mon-Sat 11:00-18:00	t
3	OAZA Joyful Kitchen	Ulica Pavla Radića 9	1	10000	45.81434540	15.97565004	0976602744	joyful-kitchen.com	Mon-Fri 10:00-21:00,Sat 12:00-21:00	t
4	BEKIND	Ilica 75	1	10000	45.81245345	15.96450714	015534763	bekind.hr	Tue-Sat 09:00-22:00,Sun 09:00-16:00	f
5	Zrno bio bistro	Medulićeva ulica 20	1	10000	45.81122113	15.96667598	014847540	zrnobiobistro.hr	Mon-Sat 12:00-21:30	t
6	Falafel, etc.	Ulica Andrije Žaje 60	1	10000	45.80695890	15.95334024	012343945	falafeletc.com.hr	Mon-Sun 11:00-21:30	t
7	Shambala	Iločka ulica 34	1	10000	45.79956603	15.95967074	0957618710	facebook.com/people/Shambala-Vegan-food-sweets-more	Mon-Fri 11:00-18:00,Sat-Sun 11:00:16:00	t
8	Simple Green by Jelena	Zelinska Ulica 7	1	10000	45.80106354	15.97246634	015561679	simplegreenbyjelena.com	Mon-Sat 08:00-16:00	t
9	Vege Fino za sve	Ulica Lavoslava Ružičke 48	1	10000	45.79626824	15.96986333	098777577	vegefinozasve.com	Mon-Fri 10:00-18:00,Sun 12:00-18:00	t
10	Barcode Mitra	Zagrebačka cesta 113	1	10000	45.80673849	15.92470464	013770428	barcodemitra.hr	Mon-Sat 14:00-22:00	t
11	Pandora Greenbox	Obrov ulica 4	2	21000	43.50930416	16.43764950	021236120	pandoragreenbox.com	Mon-Sun 08:30-12:00	f
12	Upcafe	Ulica Domovinskog rata 29a	2	21000	43.51692872	16.44540914	0916210500	upcafe.hr	Mon-Sat 07:00-20:00,Sun 08:00-20:00	t
13	The Botanist	Ulica Mihovila Pavlinovića 4	3	23000	44.11643740	15.22593932	0924232296	instagram.com/botanist_zadar	Mon-Sun 12:00-23:00	f
14	Nishta	Prijeko ulica bb	4	20000	42.67534972	18.10592518	020322088	nishtarestaurant.com	Mon-Sat 11:30-22:00	f
15	BioMania Bistro Bol	Rudina 10	5	21420	43.26175996	16.65493408	0919362276	biomania.hr	Mon-Thu 13:00-21:00,Sat 13:00-21:00,Sun 10:00-22:00	f
\.


--
-- Name: restaurant_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.restaurant_id_seq', 15, true);


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
-- Name: restaurant restaurant_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.restaurant
    ADD CONSTRAINT restaurant_pkey PRIMARY KEY (id);


--
-- Name: restaurant restaurant_cityid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.restaurant
    ADD CONSTRAINT restaurant_cityid_fkey FOREIGN KEY (cityid) REFERENCES public.city(id);


--
-- PostgreSQL database dump complete
--

