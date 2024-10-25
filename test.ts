const popperProps = {
  modifiers: {
    preventOverflow: {
      enabled: true,
      boundariesElement: 'window', // Keeps it within the window bounds
    },
    flip: {
      enabled: true, // This allows the popper to flip from bottom to top if there isn't enough space
      behavior: ['bottom', 'top'], // Try bottom first, then top
    },
  },
  placement: 'bottom-start', // Default placement to bottom
};
