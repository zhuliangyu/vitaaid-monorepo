import React, { useState, useEffect } from 'react';

const PopupAd = () => {
  const [isVisible, setIsVisible] = useState(true);
  const [isMobile, setIsMobile] = useState(false);
  const [isCanada, setIsCanada] = useState(false);

  useEffect(() => {
    const handleResize = () => {
      setIsMobile(window.innerWidth <= 440);
    };

    const country = sessionStorage.getItem('country') == null ? 'CA' : sessionStorage.getItem('country');
    setIsCanada(country === 'CA');

    const isPopupDisabled = localStorage.getItem('disablePopup');
    if (isPopupDisabled === 'true') {
      setIsVisible(false); // 如果用户选择不再显示，隐藏弹窗
    }

    handleResize(); // 初始化
    window.addEventListener('resize', handleResize);
    return () => window.removeEventListener('resize', handleResize);
  }, []);

  if (!isVisible) return null;

  const getImageSrc = (isCanada: boolean, isMobile: boolean): string => {
    if (isCanada && isMobile) {
      // CA mobile
      return '/img/VA_CA_Website_Popup_320x250_05_15_2025.jpg'; // 条件 1: isCanada 且 isMobile
    } else if (!isCanada && !isMobile) {
      // US desktop
      return '/img/VA_US_Website_Popup_1800x750_05_15_2025.jpg'; // 条件 2: US 且 非 isMobile
    } else if (isCanada && !isMobile) {
      // CA desktop
      return '/img/VA_CA_Website_Popup_1800x750_05_15_2025.jpg'; // 条件 3: isCanada 且 非 isMobile
    } else {
      // US mobile
      return '/img/VA_US_Website_Popup_320x250_2025_05_15_2025.jpg'; // 条件 4: US 且 isMobile
    }
  };

  return (
    <div
      style={{
        position: 'fixed',
        top: 0,
        left: 0,
        width: '100vw',
        height: '100vh',
        backgroundColor: 'rgba(0, 0, 0, 0.5)',
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
        zIndex: 9999,
      }}
    >
      <div
        style={{
          position: 'relative',
          width: isMobile ? '90%' : '1000px', // 移动端宽度调整为 90%
          height: isMobile ? '350px' : '500px', // 移动端高度自适应
          borderRadius: '8px',
          overflow: 'hidden',
          backgroundColor: 'rgb(0, 85, 140)',
        }}
      >
        {/* 广告图 */}
        <img
          alt="Ad"
          src={getImageSrc(isCanada, isMobile)}
          style={{
            width: '100%',
            height: isMobile ? 'auto' : 'auto', // 移动端高度自适应
            objectFit: 'cover',
          }}
        />

        {/* 关闭按钮 */}
        <button
          onClick={() => setIsVisible(false)}
          style={{
            position: 'absolute',
            top: '10px',
            right: '10px',
            backgroundColor: 'white',
            border: 'none',
            borderRadius: '50%',
            width: '30px',
            height: '30px',
            fontSize: '18px',
            cursor: 'pointer',
          }}
        >
          ✕
        </button>

        <div
          style={{
            bottom: isMobile ? '10px' : '20px',
            width: '100%',
            position: 'absolute',
            display: 'flex',
            alignItems: 'center',
            justifyContent: isMobile ? 'right' : 'center', // 水平居中
            gap: '10px',
          }}
        >
          {/* 跳转按钮 */}
          <a
            href="https://lp.constantcontactpages.com/cu/6GTRFLR/ClinicalSuccessSummit25"
            target="_blank"
            rel="noopener noreferrer"
            style={{
              // transform: isMobile ? 'none' : 'translateX(-50%)', // 移动端取消偏移
              backgroundColor: '#007bff',
              color: 'white',
              padding: '10px 20px',
              textDecoration: 'none',
              borderRadius: '4px',
              fontWeight: 'bold',
              fontSize: '14px',
            }}
          >
            Learn More
          </a>
          {/* 复选框 */}
          <div
            style={{
              position: isMobile ? 'relative' : 'absolute', // 使用绝对定位
              right: isMobile ? '10px' : '20px', // 距离右边缘的距离
              marginLeft: isMobile ? '20px' : 'none',
              color: 'black',
              cursor: 'pointer',
              display: 'flex',
              alignItems: 'right',
              gap: '10px',
            }}
          >
            <input
              type="checkbox"
              onChange={(e) => {
                if (e.target.checked) {
                  localStorage.setItem('disablePopup', 'true');
                  // Handle "Don't show this again" logic here
                  // pause 0.5 second`
                  setTimeout(() => {
                    setIsVisible(false);
                  }, 500);
                }
              }}
              style={{ cursor: 'pointer' }}
            />
            <div style={{ color: 'white' }}>Not interested</div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default PopupAd;
