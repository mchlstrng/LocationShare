var map;
var markers = {};

window.initMap = () => {
    map = L.map('mapid').setView([0, 0], 2);
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 19,
    }).addTo(map);
};

window.addMarker = (lat, lon, user) => {
    if (markers[user]) {
        map.removeLayer(markers[user]);
    }
    markers[user] = L.marker([lat, lon], { autoPan: false }).addTo(map)
        .bindPopup(user);
};

window.getUserLocation = () => {
    return new Promise((resolve, reject) => {
        navigator.geolocation.getCurrentPosition(
            position => resolve({ Latitude: position.coords.latitude, Longitude: position.coords.longitude }),
            err => reject(err.message),
            { enableHighAccuracy: true }
        );
    });
};