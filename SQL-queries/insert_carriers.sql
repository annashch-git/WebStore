-- Вставка двух перевозчиков: DHL и UPS
INSERT INTO carriers (
    carrier_name,
    contact_url,
    contact_phone
)
VALUES
    ('DHL', 'https://www.dhl.com', '+49 228 767 676'),
    ('UPS', 'https://www.ups.com', '+1 800 742 5877');

-- Проверка вставленных записей
SELECT * FROM carriers;
