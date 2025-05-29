-- Gán thời gian hiện tại cho tất cả bản ghi
SET @now = NOW();

-- ====== PHONG CÁCH DẪN CHƯƠNG TRÌNH ======
INSERT INTO hosting_style (label, created_at, modified_at, is_active)
SELECT * FROM (
    SELECT 'Nhiệt huyết' as label, @now created_at, @now modified_at, b'1' is_active
    UNION ALL SELECT 'Nhẹ nhàng', @now, @now, b'1'
    UNION ALL SELECT 'Sang trọng', @now, @now, b'1'
    UNION ALL SELECT 'Sâu sắc', @now, @now, b'1'
    UNION ALL SELECT 'Hài hước', @now, @now, b'1'
    UNION ALL SELECT 'Trẻ trung', @now, @now, b'1'
    UNION ALL SELECT 'Giàu cảm xúc', @now, @now, b'1'
    UNION ALL SELECT 'Chuyên nghiệp', @now, @now, b'1'
    UNION ALL SELECT 'Gần gũi', @now, @now, b'1'
    UNION ALL SELECT 'Lịch lãm', @now, @now, b'1'
    UNION ALL SELECT 'Sôi động', @now, @now, b'1'
    UNION ALL SELECT 'Tự nhiên', @now, @now, b'1'
) AS tmp
WHERE NOT EXISTS (
    SELECT 1 FROM hosting_style hs
    WHERE LOWER(hs.label) = LOWER(tmp.label)
);

-- ====== LOẠI MC ======
INSERT INTO mc_type (label, description, created_at, modified_at, is_active)
SELECT * FROM (
    SELECT 'MC Đám cưới' AS label, 'Chuyên dẫn các chương trình cưới hỏi' as description, @now as created_at, @now  as modified_at, b'1' as is_active
    UNION ALL SELECT 'MC Gala', 'Dẫn chương trình gala dinner, tiệc cuối năm', @now, @now, b'1'
    UNION ALL SELECT 'MC Talkshow', 'Dẫn các buổi tọa đàm, trò chuyện chuyên đề', @now, @now, b'1'
    UNION ALL SELECT 'MC Truyền hình', 'Dẫn các chương trình truyền hình', @now, @now, b'1'
    UNION ALL SELECT 'MC Giới thiệu sản phẩm', 'Sự kiện ra mắt sản phẩm, khai trương', @now, @now, b'1'
    UNION ALL SELECT 'Voice Talent', 'Lồng tiếng phim, đọc quảng cáo, video', @now, @now, b'1'
    UNION ALL SELECT 'MC Team Building', 'Dẫn các hoạt động tập thể ngoài trời', @now, @now, b'1'
    UNION ALL SELECT 'MC Thiếu nhi', 'Dẫn các chương trình dành cho trẻ em', @now, @now, b'1'
    UNION ALL SELECT 'MC Ca nhạc', 'Dẫn đêm nhạc, sự kiện âm nhạc', @now, @now, b'1'
    UNION ALL SELECT 'MC Sự kiện doanh nghiệp', 'Lễ ký kết, hội nghị khách hàng...', @now, @now, b'1'
    UNION ALL SELECT 'MC Hội thảo', 'Dẫn các hội thảo chuyên đề chuyên môn', @now, @now, b'1'
    UNION ALL SELECT 'MC Thời trang', 'Dẫn chương trình biểu diễn thời trang', @now, @now, b'1'
    UNION ALL SELECT 'MC Livestream', 'Dẫn livestream bán hàng, giới thiệu sản phẩm', @now, @now, b'1'
    UNION ALL SELECT 'MC Game show', 'Dẫn các chương trình game show, minigame', @now, @now, b'1'
    UNION ALL SELECT 'MC Đối ngoại/Thông dịch', 'Dẫn chương trình có yếu tố quốc tế, cần thông dịch', @now, @now, b'1'
) AS tmp
WHERE NOT EXISTS (
    SELECT 1 FROM mc_type mt
    WHERE LOWER(mt.label) = LOWER(tmp.label)
);
