db.createUser(
    {
        user: "mongoadmin",
        pwd: "password",
        roles: [
            {
                role: "readWrite",
                db: "catalog"
            }
        ]
    }
);
db.createCollection("test"); //MongoDB creates the database when you first store data in that database