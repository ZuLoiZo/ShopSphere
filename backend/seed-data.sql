-- Seed Categories
INSERT INTO "Categories" ("Name", "Slug", "Description", "CreatedAt") VALUES
('Électronique', 'electronique', 'Produits électroniques et high-tech', NOW()),
('Vêtements', 'vetements', 'Mode et accessoires', NOW()),
('Livres', 'livres', 'Livres et magazines', NOW()),
('Sport', 'sport', 'Articles de sport et fitness', NOW());

-- Seed Products
INSERT INTO "Products" ("Name", "Description", "Price", "Stock", "ImageUrl", "CategoryId", "IsActive", "CreatedAt", "UpdatedAt") VALUES
('MacBook Pro 14"', 'Ordinateur portable Apple avec puce M3', 2499.99, 10, 'https://via.placeholder.com/300/3B82F6/FFFFFF?text=MacBook', 1, true, NOW(), NOW()),
('iPhone 15 Pro', 'Smartphone Apple dernière génération', 1199.99, 25, 'https://via.placeholder.com/300/3B82F6/FFFFFF?text=iPhone', 1, true, NOW(), NOW()),
('T-Shirt Nike', 'T-shirt de sport confortable', 29.99, 100, 'https://via.placeholder.com/300/10B981/FFFFFF?text=T-Shirt', 2, true, NOW(), NOW()),
('Jean Levi''s 501', 'Jean classique coupe droite', 89.99, 50, 'https://via.placeholder.com/300/10B981/FFFFFF?text=Jean', 2, true, NOW(), NOW()),
('Clean Code', 'Livre de Robert C. Martin sur le code propre', 39.99, 30, 'https://via.placeholder.com/300/F59E0B/FFFFFF?text=Book', 3, true, NOW(), NOW()),
('Haltères 10kg', 'Paire d''haltères réglables', 79.99, 15, 'https://via.placeholder.com/300/EF4444/FFFFFF?text=Dumbbell', 4, true, NOW(), NOW());
