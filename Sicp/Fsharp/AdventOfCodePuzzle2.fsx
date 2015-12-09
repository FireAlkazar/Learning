﻿// Day 9
type LocationsRoutes = Map<string*string,int>

let getDistanceBetween (city1:string) (city2:string) (routes:LocationsRoutes) =
    if Map.containsKey (city1,city2) routes then
        routes.[(city1,city2)]
    elif Map.containsKey (city2,city1) routes then
        routes.[(city2,city1)]
    else
        failwith "Route not found"

let getAllCities (routes:LocationsRoutes) =
    Map.toList routes
    |> List.map fst
    |> List.collect (fun x -> [fst x;snd x])
    |> Seq.distinct
    |> Seq.toList

let getNearestCity (city:string) (routes:LocationsRoutes)(cityList: string list) =
    let mutable nearestCity = ""
    let mutable minDistance = System.Int32.MaxValue
    for curCity in cityList do
        let curDistance = getDistanceBetween city curCity routes
        if curDistance < minDistance then
            nearestCity <- curCity
            minDistance <- curDistance
    (nearestCity, minDistance)

let getFarestCity (city:string) (routes:LocationsRoutes)(cityList: string list) =
    let mutable farestCity = ""
    let mutable maxDistance = 0
    for curCity in cityList do
        let curDistance = getDistanceBetween city curCity routes
        if curDistance > maxDistance then
            farestCity <- curCity
            maxDistance <- curDistance
    (farestCity, maxDistance)

let rec getShortestPathWithStartCity (startCity:string) (routes:LocationsRoutes) (cityList: string list) =
    if cityList = [] then
        0
    else
        let nearest = getNearestCity startCity routes cityList
        let visitedCity = (fst nearest)
        let distance = snd nearest
        let newCityList = cityList |> List.filter ((<>) visitedCity)
        distance + (getShortestPathWithStartCity visitedCity routes newCityList)

let rec getLongestPathWithStartCity (startCity:string) (routes:LocationsRoutes) (cityList: string list) =
    if cityList = [] then
        0
    else
        let farest = getFarestCity startCity routes cityList
        let visitedCity = (fst farest)
        let distance = snd farest
        let newCityList = cityList |> List.filter ((<>) visitedCity)
        distance + (getLongestPathWithStartCity visitedCity routes newCityList)

let routesNative =
    [
    (("AlphaCentauri","Snowdin"),66)
    (("AlphaCentauri","Tambi"),28)
    (("AlphaCentauri","Faerun"),60)
    (("AlphaCentauri","Norrath"),34)
    (("AlphaCentauri","Straylight"),34)
    (("AlphaCentauri","Tristram"),3)
    (("AlphaCentauri","Arbre"),108)
    (("Snowdin","Tambi"),22)
    (("Snowdin","Faerun"),12)
    (("Snowdin","Norrath"),91)
    (("Snowdin","Straylight"),121)
    (("Snowdin","Tristram"),111)
    (("Snowdin","Arbre"),71)
    (("Tambi","Faerun"),39)
    (("Tambi","Norrath"),113)
    (("Tambi","Straylight"),130)
    (("Tambi","Tristram"),35)
    (("Tambi","Arbre"),40)
    (("Faerun","Norrath"),63)
    (("Faerun","Straylight"),21)
    (("Faerun","Tristram"),57)
    (("Faerun","Arbre"),83)
    (("Norrath","Straylight"),9)
    (("Norrath","Tristram"),50)
    (("Norrath","Arbre"),60)
    (("Straylight","Tristram"),27)
    (("Straylight","Arbre"),81)
    (("Tristram","Arbre"),90)
    ]

let getShortestPath =
    let routes = LocationsRoutes(routesNative)
    let allCities = getAllCities routes
    let mutable minDistance = System.Int32.MaxValue
    for curCity in allCities do
        let citiesToVisit = allCities |> List.filter ((<>) curCity)
        let curCityMin = getShortestPathWithStartCity curCity routes citiesToVisit
        minDistance <- min minDistance curCityMin
    minDistance

let getLongestPath =
    let routes = LocationsRoutes(routesNative)
    let allCities = getAllCities routes
    let mutable maxDistance = 0
    for curCity in allCities do
        let citiesToVisit = allCities |> List.filter ((<>) curCity)
        let curCityMax = getLongestPathWithStartCity curCity routes citiesToVisit
        maxDistance <- max maxDistance curCityMax
    maxDistance