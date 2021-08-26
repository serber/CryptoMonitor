let res = [
  db.createCollection('user');
  db.createCollection('symbol_price');
  db.createCollection('drop_price');
];

printjson(res);