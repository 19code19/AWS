// Updated block and unblock navigation logic
let unBlock: any = null;

export const blockNavigation = () => {
  if (!unBlock) {
    unBlock = history.block();
  }
};

export const unBlockNavigation = () => {
  if (unBlock) {
    unBlock();
    unBlock = null;
  }
};

// Test cases
describe('Navigation Blocking', () => {
  let mockBlock: jest.SpyInstance;

  beforeEach(() => {
    mockBlock = jest.spyOn(history, 'block').mockImplementation(() => jest.fn());
    unBlock = null; // Reset unBlock before each test
  });

  afterEach(() => {
    jest.clearAllMocks(); // Ensure all mocks are cleared after each test
  });

  test('blockNavigation should call history.block if unBlock is null', () => {
    blockNavigation();
    expect(mockBlock).toHaveBeenCalled();
  });

  test('blockNavigation should not call history.block again if unBlock is not null', () => {
    blockNavigation(); // First call to set unBlock
    blockNavigation(); // Second call should not call history.block again
    expect(mockBlock).toHaveBeenCalledTimes(1);
  });

  test('unBlockNavigation should call unBlock and set it to null', () => {
    blockNavigation(); // Set unBlock
    unBlockNavigation(); // Call unBlock
    expect(mockBlock.mock.results[0].value).toHaveBeenCalled(); // Check if unBlock is called
    expect(unBlock).toBeNull(); // Ensure unBlock is null after being called
  });

  test('unBlockNavigation should not call unBlock if it is null', () => {
    unBlockNavigation(); // unBlock is already null
    expect(mockBlock).not.toHaveBeenCalled(); // Should not call history.block
  });
});
