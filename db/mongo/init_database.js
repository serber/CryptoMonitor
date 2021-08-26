let res = [
  db.createCollection('user'),
  db.createCollection('symbol_price'),
  db.createCollection('drop_price'),
  
  db.user.insert(
  {
    "_id": "c11bb741-746f-4b98-8c33-264047d55411",
    "login": "admin",
    "password_hash": "l2Y5necZdbFHbwBHjLsXgKsL6bgyzYReWdUv5QzEjyg="
  })
];

printjson(res);